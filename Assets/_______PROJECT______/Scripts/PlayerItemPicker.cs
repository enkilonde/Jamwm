using UnityEngine;

public class PlayerItemPicker : MonoBehaviour {

    [SerializeField] private CustomCharacterController _character;

    void OnTriggerEnter(Collider other) {
        if (other == null) {
            Debug.Log("?");
            return;
        }

        var itemSheet = other.GetComponent<Item>();
        if (itemSheet == null) {
            return;
        }

        if (itemSheet.Equipped) {
            return;
        }

        Debug.LogError("START Collide with " + itemSheet.name);
    }

    private void OnTriggerExit(Collider other) {
        if (other == null) {
            Debug.Log("?");
            return;
        }

        var itemSheet = other.GetComponent<Item>();
        if (itemSheet == null) {
            return;
        }

        Debug.LogError("EXIT Collide with " + itemSheet.name);
    }

}