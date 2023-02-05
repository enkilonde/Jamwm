using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class RoomManager : MonoBehaviour {

    public static RoomManager Instance;

    [Header("Data")]
    [SerializeField] public BossRoomController _currentRoom;

    [Header("Other Managers")]
    [SerializeField] public Transform _playerTransform;
    [SerializeField] private CharacterController _playerController;
    [SerializeField] private CustomCharacterController _playerCustomController;
    [SerializeField] private TransitionUiController _transitionUi;
    [SerializeField] private CinemachineTargetGroup _cameraGroup;

    [Header("UI")]
    [SerializeField] private LifeBar _playerLifeBar;
    [SerializeField] private LifeBar _bossLifeBar;

    [Header("Resources")]
    [SerializeField] private RoomDatabase _possibleRoomsPool;

    // TODO : might have to be kept elsewhere
    public int CurrentLevel { get; private set; }

    // Temporary use field
    private readonly List<Behaviour> _disabledDuringTransition = new List<Behaviour>();

    private void Awake() {
        Instance = this;
    }

#region Initial Room

    private void Start() {
        if (CheatsManager.Instance.QuickTesting) {
            _playerLifeBar.InitPlayerBar(_playerCustomController.CharacterSheet.MaxHp);
            LoadRoom(0, AncestorGenerator.Instance.GenerateAncestor(0));
        } else {
            LoadInitialRoom();
        }
    }

    private void LoadInitialRoom() {
        CurrentLevel = 0;

        (AncestorData, AncestorData) parents = AncestorGenerator.Instance.GetInitialParents();
        _currentRoom.Configure(
            AncestorGenerator.Instance.GenerateAncestor(0),
            parents.Item1,
            parents.Item2,
            _playerCustomController
        );

        _playerTransform.position = _currentRoom.PlayerSpawnPoint;

        _currentRoom.SetExitDoorsLocked(false);

        _playerLifeBar.InitPlayerBar(_playerCustomController.CharacterSheet.MaxHp);
    }

#endregion

    public void HandleBossHealthZero() {
        SaveManager.Instance.HandleDefeatedAncestor(_currentRoom.Ancestor);
        _currentRoom.SetExitDoorsLocked(false);

        // Clearing the "dual camera targetting"
        _cameraGroup.m_Targets = new CinemachineTargetGroup.Target[] {
            new CinemachineTargetGroup.Target() {
                target = _playerController.transform,
                weight = 1,
                radius = 0
            }
        };

        // Gathering the boss data
        CustomCharacterController bossObject = _currentRoom.BossRef;
        Vector3 corpsePosition = bossObject.transform.position;
        BossSheet bossSheet = (BossSheet) bossObject.CharacterSheet;
        List<Item> bossLoots = bossSheet.BossLoots();

        // Destroying the corpse - TODO : animate the death / SFX / VFX / ...
        if (bossObject.leftWeapon != null) {
            Destroy(bossObject.leftWeapon.armBehaviour.gameObject);
        }
        if (bossObject.rightWeapon != null) {
            Destroy(bossObject.rightWeapon.armBehaviour.gameObject);
        }
        Destroy(bossObject.gameObject);

        LootSpawner.Instance.SpawnLoots(bossLoots, corpsePosition);

        _bossLifeBar.FadeTo(visible: false, null);
    }

    public void TransitionToNextBossRoom(AncestorData chosenAncestor) {
        LoadRoom(CurrentLevel + 1, chosenAncestor);
    }

#region Room Swapping

    private void UnloadRoom() {
        Destroy(_currentRoom.gameObject);
        _currentRoom = null;
    }

    private void LoadRoom(int roomLevel, AncestorData chosenAncestor) {
        // Pre calculations
        (AncestorData, AncestorData) parents = AncestorGenerator.Instance.GetParents(chosenAncestor);

        _disabledDuringTransition.Clear();
        _disabledDuringTransition.Add(_playerCustomController);
        _disabledDuringTransition.Add(_playerController.GetComponent<PlayerInput>());
        _disabledDuringTransition.Add(_playerController.GetComponent<CustomPlayerInputs>());

        foreach (var component in _disabledDuringTransition) {
            component.enabled = false;
        }

        // Launching the Room transition
        _transitionUi.Transition(
            // Halfway through, we ACTUALLY swap the rooms
            halfwayAction: () => {
                UnloadRoom();
                CurrentLevel = roomLevel;

                _currentRoom = Instantiate(_possibleRoomsPool.GetRandomRoomModel());

                _currentRoom.Configure(
                    chosenAncestor,
                    parents.Item1,
                    parents.Item2,
                    _playerCustomController
                );

                _currentRoom.SetBossAiEnabled(false);

                _bossLifeBar.SetBossName(chosenAncestor.Name);

                _playerTransform.position = _currentRoom.PlayerSpawnPoint;
            },

            // When the transition is finished, we resume gameplay
            callbackAction: () => {
                _bossLifeBar.SetValue(1);
                _bossLifeBar.FadeTo(visible: true, MakeCameraSlide);
            }
        );
    }

    private void MakeCameraSlide() {
        _cameraGroup.m_Targets = new CinemachineTargetGroup.Target[] {
            new CinemachineTargetGroup.Target() {
                target = _playerController.transform,
                weight = 1,
                radius = 0
            },
            new CinemachineTargetGroup.Target() {
                target = _currentRoom.BossRef.transform,
                weight = 0,
                radius = 0
            },
        };

        float w = 0;
        var anim = DOTween.Sequence();
        anim.Append(DOTween.To(
            getter: () => w,
            setter: (value) => {
                w = value;
                _cameraGroup.m_Targets[0].weight = 1 - w;
                _cameraGroup.m_Targets[1].weight = w;
            },
            endValue: 1,
            duration: 0.5f
        ).SetEase(Ease.InOutCubic));
        anim.AppendInterval(1f);
        anim.Append(DOTween.To(
            getter: () => w,
            setter: (value) => {
                w = value;
                _cameraGroup.m_Targets[0].weight = 1 - w;
            },
            endValue: 0,
            duration: 0.35f
        ).SetEase(Ease.InCubic));
        anim.AppendInterval(0.8f);
        anim.onComplete += HandleCameraSlideEnd;
    }

    private void HandleCameraSlideEnd() {
        foreach (var component in _disabledDuringTransition) {
            component.enabled = true;
        }
        _disabledDuringTransition.Clear();
        _currentRoom.SetBossAiEnabled(true);
    }

#endregion

#region Cheats

    public void ReShuffleNextAncestors() {
        (AncestorData, AncestorData) newOptions = AncestorGenerator.Instance.GetParents(_currentRoom.Ancestor);

        _currentRoom.Configure(
            _currentRoom.Ancestor,
            newOptions.Item1,
            newOptions.Item2,
            _playerCustomController
        );
    }

#endregion

}