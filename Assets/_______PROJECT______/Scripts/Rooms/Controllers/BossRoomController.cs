using UnityEngine;

public class BossRoomController : MonoBehaviour {

    [SerializeField] private DoorController _leftExitDoor;
    [SerializeField] private DoorController _rightExitDoor;

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
        // TODO
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