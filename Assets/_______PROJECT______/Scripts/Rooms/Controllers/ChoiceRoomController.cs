using System;
using UnityEngine;

public class ChoiceRoomController : GenericRoomController {

    protected override RoomType RoomType => RoomType.Choice;

    public override int Width => 60;
    public override int Depth => 20;
    public override int Height => 5;

    [SerializeField] private DoorController _entryDoor;
    [SerializeField] private DoorController _leftExitDoor;
    [SerializeField] private DoorController _rightExitDoor;

    public override void SetDoorsLocked(DoorController.DoorKind doorKind, bool locked) {
        switch (doorKind) {
            case DoorController.DoorKind.Entry:
                _entryDoor.SetLocked(locked);
                break;
            case DoorController.DoorKind.Exit:
                _leftExitDoor.SetLocked(locked);
                _rightExitDoor.SetLocked(locked);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(doorKind), doorKind, null);
        }
    }

}