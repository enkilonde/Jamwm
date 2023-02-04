using UnityEngine;

public class PlayerItemPicker : MonoBehaviour {

    [SerializeField] private CustomCharacterController _character;

    void OnTriggerEnter(Collider other) {
        if (other == null) {
            Debug.Log("?");
            return;
        }

        var lootable = other.GetComponent<LootableItem>();
        if (lootable == null) {
            return;
        }

        Debug.Log("Picking up " + lootable.Loot.Name);
        // TODO : only do that after a player confirmation
        ((PlayerSheet)_character.CharacterSheet).PickUp(lootable);

        // Making the Lootable disappear
        lootable.enabled = false;
        Destroy(other.gameObject);
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
    }

}