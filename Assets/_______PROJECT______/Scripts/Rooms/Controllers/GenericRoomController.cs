using UnityEngine;

public abstract class GenericRoomController : MonoBehaviour {

    protected abstract RoomType RoomType { get; }

    public abstract int Width { get; }
    public abstract int Depth { get; }
    public abstract int Height { get; }

    public virtual void HandleDoorPassed(DoorController.DoorKind doorKind) {
        if (doorKind == DoorController.DoorKind.Entry) {
            SetDoorsLocked(DoorController.DoorKind.Entry, true);
        }
        if (doorKind == DoorController.DoorKind.Exit) {
            RoomManager.Instance.UnloadRoom(this.RoomType);
        }
    }

    public abstract void SetDoorsLocked(DoorController.DoorKind doorKind, bool locked);

}