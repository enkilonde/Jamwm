using UnityEngine;

public class PlayerItemPicker : MonoBehaviour {

    [SerializeField] private CustomCharacterController _character;

    void OnTriggerEnter(Collider other) {
        if (other == null) {
            Debug.Log("?");
            return;
        }

        var loot = other.GetComponent<Item>();
        if (loot == null) {
            return;
        }

        if (loot.Equipped) {
            return;
        }

        Debug.LogError("Can pick up " + loot.Name);
        // TODO : only do that after a player confirmation
        /*((PlayerSheet)_character.CharacterSheet).Equip(loot);

        Destroy(loot.gameObject);*/
    }

    private void OnTriggerExit(Collider other) {
        if (other == null) {
            Debug.Log("?");
            return;
        }

        var lootSheet = other.GetComponent<Item>();
        if (lootSheet == null) {
            return;
        }
    }

}