using System;
using UnityEngine;

public class BossRoomController : GenericRoomController {

    protected override RoomType RoomType => RoomType.Combat;

    public override int Width => 60;
    public override int Depth => 60;
    public override int Height => 5;

    [SerializeField] private DoorController _entryDoor;
    [SerializeField] private DoorController _exitDoor;

    public override void HandleDoorPassed(DoorController.DoorKind doorKind) {
        if (doorKind == DoorController.DoorKind.Entry) {
            this.SetDoorsLocked(DoorController.DoorKind.Exit, true);
        }
        if (doorKind == DoorController.DoorKind.Exit) {
            RoomManager.Instance.HandleBossRoomLeft();
        }

        base.HandleDoorPassed(doorKind);
    }

    public override void SetDoorsLocked(DoorController.DoorKind doorKind, bool locked) {
        switch (doorKind) {
            case DoorController.DoorKind.Entry:
                _entryDoor.SetLocked(locked);
                break;
            case DoorController.DoorKind.Exit:
                _exitDoor.SetLocked(locked);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(doorKind), doorKind, null);
        }
    }

}