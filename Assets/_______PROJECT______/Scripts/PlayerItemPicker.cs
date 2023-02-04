using UnityEngine;

public class PlayerItemPicker : MonoBehaviour {

    [SerializeField] private CustomCharacterController _character;

    void OnTriggerEnter(Collider other) {
        if (other == null) {
            Debug.Log("?");
            return;
        }

        var lootSheet = other.GetComponent<LootableItem>();
        if (lootSheet == null) {
            return;
        }

        Debug.LogError("START Collide with " + lootSheet.name);
    }

    private void OnTriggerExit(Collider other) {
        if (other == null) {
            Debug.Log("?");
            return;
        }

        var lootSheet = other.GetComponent<LootableItem>();
        if (lootSheet == null) {
            return;
        }

        Debug.LogError("EXIT Collide with " + lootSheet.name);
    }

}