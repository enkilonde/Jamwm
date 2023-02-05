using System;
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

        ItemSlot slot = GetSlotForItemKind(loot.Kind);
        Item currentItem = null;
        if (_character.CharacterSheet.Equipment.ContainsKey(slot))
            currentItem = _character.CharacterSheet.Equipment[slot];

        TempLootUi.Instance.Configure(loot, currentItem);
        TempLootUi.Instance.SetVisible(true);

        // TODO : only do that after a player confirmation
        /*((PlayerSheet)_character.CharacterSheet).Equip(loot);

        Destroy(loot.gameObject);*/
    }

    private ItemSlot GetSlotForItemKind(ItemKind kind) {
        switch (kind) {
            case ItemKind.Helmet:
                return ItemSlot.Head;
            case ItemKind.Armor:
                return ItemSlot.Torso;
            case ItemKind.Ring:
                return ItemSlot.Ring1;
            case ItemKind.Weapon:
                var equipment = _character.CharacterSheet.Equipment;
                if (equipment.ContainsKey(ItemSlot.LeftArm) && equipment[ItemSlot.LeftArm] != null) {
                    return ItemSlot.RightArm;
                }
                return ItemSlot.LeftArm;
            default:
                throw new ArgumentOutOfRangeException(nameof(kind), kind, null);
        }
    }

    private void OnTriggerExit(Collider other) {
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

        TempLootUi.Instance.ForgetItem(loot);
    }

}