using UnityEngine;

public class RoomManager : MonoBehaviour {

    public static RoomManager Instance;

    [Header("Data")]
    [SerializeField] public BossRoomController _currentRoom;

    [Header("Other Managers")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private CharacterController _playerController;
    [SerializeField] private CustomCharacterController _playerCustomController;
    [SerializeField] private TransitionUiController _transitionUi;

    [Header("UI")]
    [SerializeField] private LifeBar _playerLifeBar;
    [SerializeField] private LifeBar _bossLifeBar;

    [Header("Resources")]
    [SerializeField] private BossRoomController _bossRoomPrefab;

    // TODO : might have to be kept elsewhere
    public int CurrentLevel { get; private set; }

    private void Awake() {
        Instance = this;
    }

#region Initial Room

    private void Start() {
        LoadInitialRoom();
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

        _playerTransform.position = Vector3.zero;

        _playerLifeBar.InitPlayerBar(_playerCustomController.CharacterSheet.MaxHp);
    }

#endregion

    public void HandleBossHealthZero() {
        SaveManager.Instance.HandleDefeatedAncestor(_currentRoom.Ancestor);
        _currentRoom.SetExitDoorsLocked(false);

        // TODO : animate the death / SFX / VFX / ...
        var bossObject = _currentRoom.BossRef;
        if (bossObject.leftWeapon != null) {
            Destroy(bossObject.leftWeapon.armBehaviour.gameObject);
        }
        if (bossObject.rightWeapon != null) {
            Destroy(bossObject.rightWeapon.armBehaviour.gameObject);
        }
        Destroy(bossObject.gameObject);

        _bossLifeBar.FadeTo(visible: false);
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

        // Launching the Room transition
        _playerController.enabled = false;
        _transitionUi.Transition(
            // Halfway through, we ACTUALLY swap the rooms
            halfwayAction: () => {
                UnloadRoom();
                CurrentLevel = roomLevel;

                _currentRoom = Instantiate(_bossRoomPrefab);

                _currentRoom.Configure(
                    chosenAncestor,
                    parents.Item1,
                    parents.Item2,
                    _playerCustomController
                );

                _bossLifeBar.SetBossName(chosenAncestor.Name);

                _playerTransform.position = new Vector3(0, 0, -20f);
            },

            // When the transition is finished, we resume gameplay
            callbackAction: () => {
                _bossLifeBar.FadeTo(visible: true);
                _playerController.enabled = true;
            }
        );
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