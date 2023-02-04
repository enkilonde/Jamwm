using UnityEngine;

public class BossRoomController : MonoBehaviour {

    [Header("Room parts")]
    [SerializeField] private DoorController _leftExitDoor;
    [SerializeField] private DoorController _rightExitDoor;

    [Header("Resources")]
    [SerializeField] private GameObject _enemyPrefab;

    private AncestorData _leftDoorChoice;
    private AncestorData _rightDoorChoice;

    public void Configure(AncestorData boss, AncestorData leftChoice, AncestorData rightChoice) {
        _leftDoorChoice = leftChoice;
        _rightDoorChoice = rightChoice;

        if (boss.InitialRoomAncestor == false) {
            SpawnAncestor(boss);
        }
    }

    private void SpawnAncestor(AncestorData ancestorData) {
        var ancestor = Instantiate(
            original: _enemyPrefab,
            position: new Vector3(0, 0, 15),
            rotation: Quaternion.identity
        );

        var ancestorController = ancestor.GetComponent<CustomCharacterController>();
        var bossSheet = ancestorData.GetDetailedSheet(ancestorController.playerVisual);
        ancestorController.SetBossSheet(bossSheet);
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

}