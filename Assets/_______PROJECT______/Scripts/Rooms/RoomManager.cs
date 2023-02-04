using UnityEngine;

public class RoomManager : MonoBehaviour {

    public static RoomManager Instance;

    [Header("Other Managers")]
    [SerializeField] private Transform _playerTransform;

    [Header("Resources")]
    [SerializeField] private BossRoomController _bossRoomPrefab;

    private BossRoomController _currentRoom;

    // TODO : might have to be kept elsewhere
    public int CurrentLevel;

    private void Awake() {
        Instance = this;
    }

#region Initial Room

    private void Start() {
        LoadInitialRoom();
    }

    private void LoadInitialRoom() {
        CurrentLevel = 0;

        _currentRoom = Instantiate(_bossRoomPrefab);

        (AncestorData, AncestorData) parents = AncestorGenerator.Instance.GetInitialParents();
        _currentRoom.Configure(null, parents.Item1, parents.Item2);

        _playerTransform.position = Vector3.zero;
    }

#endregion

    // TODO : call it on enemy's death
    public void HandleBossDeath() {
        _currentRoom.SetExitDoorsLocked(false);
    }

    public void TransitionToNextBossRoom(AncestorData chosenAncestor) {
        // TODO : UI Fade Out
        UnloadRoom();

        // TODO : UI Fade In
        // TODO : short walk/run animation
        LoadRoom(CurrentLevel + 1, chosenAncestor);
    }

#region Room Swapping

    private void UnloadRoom() {
        Destroy(_currentRoom);
    }

    private void LoadRoom(int roomLevel, AncestorData chosenAncestor) {
        CurrentLevel = roomLevel;

        _currentRoom = Instantiate(_bossRoomPrefab);

        (AncestorData, AncestorData) parents = AncestorGenerator.Instance.GetParents(chosenAncestor);
        _currentRoom.Configure(chosenAncestor, parents.Item1, parents.Item2);

        TempUiController.Instance.UpdateAncestorName(chosenAncestor.Name);
        TempUiController.Instance.UpdateRoomLevel(CurrentLevel);

        _playerTransform.position = Vector3.zero;
    }

#endregion

}