using UnityEngine;

public class DoorController : MonoBehaviour {

    public enum DoorKind {

        Entry,
        LeftExit,
        RightExit,

    }

    [SerializeField] private BossRoomController _room;
    [SerializeField] private BoxCollider _collider;
    [SerializeField] private DoorKind _doorKind;
    [SerializeField] private bool _locked = true;

    private void OnTriggerEnter(Collider other) {
        // We only care about detecting the player's movement;
        if (other.GetComponent<CustomCharacterController>() == null) return;

        if (_locked) return;

        // Just to avoid multiple events' being sent
        this.enabled = false;

        _room.HandleDoorPassed(_doorKind);
    }

    public void SetLocked(bool locked) {
        _collider.isTrigger = locked;
        _locked = locked;
    }

}