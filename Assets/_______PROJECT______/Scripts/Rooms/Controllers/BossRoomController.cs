using UnityEngine;

public class BossRoomController : MonoBehaviour {

    [Header("Room parts")]
    [SerializeField] private DoorController _leftExitDoor;
    [SerializeField] private DoorController _rightExitDoor;

    [Header("Room configuration")]
    [SerializeField] private Vector3 BossSpawnPoint;
    [SerializeField] public Vector3 PlayerSpawnPoint;

    [Header("Resources")]
    [SerializeField] private GameObject _enemyPrefab;

    [HideInInspector]
    public CustomCharacterController BossRef;
    private CustomCharacterController _playerRef;

    public AncestorData Ancestor;
    private AncestorData _leftDoorChoice;
    private AncestorData _rightDoorChoice;

    public void Configure(AncestorData boss, AncestorData leftChoice, AncestorData rightChoice, 
        CustomCharacterController playerRef) {
        _leftDoorChoice = leftChoice;
        _rightDoorChoice = rightChoice;
        _playerRef = playerRef;

        // This weird check only serve to exclude the cheat option "reshuffle current room"
        if (boss != Ancestor) {
            Ancestor = boss;
            if ((boss.InitialRoomAncestor == false) || CheatsManager.Instance.QuickTesting) {
                SpawnAncestor(boss);
            }
        }
    }

    private void SpawnAncestor(AncestorData ancestorData) {
        var ancestor = Instantiate(
            original: _enemyPrefab,
            position: BossSpawnPoint,
            rotation: Quaternion.Euler(0, 180, 0)
        );

        var iaInputs = ancestor.GetComponent<CustomIAInputs>();
        iaInputs.player = _playerRef;
        var iaInputsMove = ancestor.GetComponent<CustomIAInputsMove>();
        iaInputsMove.player = _playerRef;

        BossRef = ancestor.GetComponent<CustomCharacterController>();
        var bossSheet = ancestorData.GetDetailedSheet(BossRef);
        BossRef.SetBossSheet(bossSheet);
    }

    public void SetExitDoorsLocked(bool locked) {
        _leftExitDoor.SetLocked(locked);
        _rightExitDoor.SetLocked(locked);
    }

    public void HandleDoorPassed(DoorController.DoorKind doorKind) {
        switch (doorKind) {
            case DoorController.DoorKind.LeftExit:
                RoomManager.Instance.TransitionToNextBossRoom(_leftDoorChoice);
                break;
            case DoorController.DoorKind.RightExit:
                RoomManager.Instance.TransitionToNextBossRoom(_rightDoorChoice);
                break;
        }
    }

    public void SetBossAiEnabled(bool active) {
        if(BossRef == null) return;
        BossRef.enabled = active;
        BossRef.GetComponent<CustomIAInputs>().enabled = active;
    }

}