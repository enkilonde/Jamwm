using UnityEngine;

public class DoorController : MonoBehaviour {

    public enum DoorKind {

        Entry,
        Exit

    }
    
    [SerializeField] private GenericRoomController _room;
    [SerializeField] private DoorKind _doorKind;
    [SerializeField] private bool _locked = true;
    

    private void OnTriggerEnter(Collider other) {
        // We only care about detecting the player's movement;
        if (other.GetComponent<CustomCharacterController>() == null) return;

        _room.HandleDoorPassed(_doorKind);
    }

    public void SetLocked(bool locked) {
        _locked = locked;
    }

}