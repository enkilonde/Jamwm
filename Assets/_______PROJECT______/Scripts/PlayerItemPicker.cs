using System;
using UnityEngine;

public class PlayerItemPicker : MonoBehaviour {

    public static PlayerItemPicker Instance;

    [SerializeField] private CustomCharacterController _character;

    public Item Lootable { get; private set; }

    private void Awake() {
        Instance = this;
    }

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

        ItemSlot slot = _character.CharacterSheet.GetSlotFromKind(loot.Kind);
        Item currentItem = null;
        if (_character.CharacterSheet.Equipment.ContainsKey(slot))
            currentItem = _character.CharacterSheet.Equipment[slot];

        TempLootUi.Instance.Configure(loot, currentItem);
        TempLootUi.Instance.SetVisible(true);

        Lootable = loot;
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
        if (Lootable == loot) {
            Lootable = null;
        }
    }

    public void ValidateLoot() {
        ((PlayerSheet) _character.CharacterSheet).Equip(Lootable);
        TempLootUi.Instance.ForgetItem(Lootable);

        Destroy(Lootable.gameObject);

        Lootable = null;
    }

}