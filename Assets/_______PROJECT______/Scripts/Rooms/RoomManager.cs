using UnityEngine;

public class RoomManager : MonoBehaviour {

    public static RoomManager Instance;

    [Header("Data")]
    [SerializeField] private BossRoomController _currentRoom;

    [Header("Other Managers")]
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private CharacterController _playerController;
    [SerializeField] private CustomCharacterController _playerCustomController;
    [SerializeField] private TransitionUiController _transitionUi;

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
    }

#endregion

    // TODO : call it on enemy's death
    public void HandleBossDeath() {
        _currentRoom.SetExitDoorsLocked(false);
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

                TempUiController.Instance.UpdateAncestorName(chosenAncestor.Name);
                TempUiController.Instance.UpdateRoomLevel(CurrentLevel);

                _playerTransform.position = new Vector3(0, 0, -20f);
            },

            // When the transition is finished, we resume gameplay
            callbackAction: () => {
                _playerController.enabled = true;
            }
        );
    }

#endregion

}