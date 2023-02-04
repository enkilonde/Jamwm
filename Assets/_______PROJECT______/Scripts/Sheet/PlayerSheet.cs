using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSheet : CharacterSheet {

    private readonly Transform _playerTransform;

#region Initialization

    public PlayerSheet(PlayerVisual playerVisual, Transform playerTransform) {
        PlayerVisual = playerVisual;
        _playerTransform = playerTransform;
        Stats = GetInitialStats();
        Equipment = new Dictionary<ItemSlot, Item>();
    }

    private Dictionary<PlayerStats, int> GetInitialStats() {
        return new Dictionary<PlayerStats, int> {
            {PlayerStats.Strength, 1},
            {PlayerStats.MagicPower, 1},
            {PlayerStats.AttackSpeed, 1},
            {PlayerStats.MovementSpeed, 1},
            {PlayerStats.Defense, 1},
            {PlayerStats.MaxHp, 10}
        };
    }

#endregion

#region Inventory

    public void PickUp(LootableItem lootable) {
        ItemSlot slot;
        var itemKind = lootable.Loot.Kind;
        switch (itemKind) {
            case ItemKind.Helmet:
                slot = ItemSlot.Head;
                break;
            case ItemKind.Armor:
                slot = ItemSlot.Torso;
                break;
            case ItemKind.Ring:
                slot = ItemSlot.Ring1;
                break;
            case ItemKind.Weapon:
                if (Equipment.ContainsKey(ItemSlot.LeftArm) && Equipment[ItemSlot.LeftArm] != null) {
                    slot = ItemSlot.RightArm;
                } else {
                    slot = ItemSlot.LeftArm;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        Equip(slot, lootable.Loot);
    }

    public void Equip(ItemSlot slot, Item item) {
        // Optional phase 1 : drop a replaced item
        if (Equipment.ContainsKey(slot)) {
            // Data update
            var droppedItem = DropItem(slot);

            // Visual update
            PlayerVisual.ClearSlot(slot);
            LootSpawner.Instance.SpawnLoot(droppedItem, _playerTransform.position);
        }

        // Main Phase 2 : equip the item

        // Data update
        Equipment[slot] = item;
        item.Equipped = true;
        RefreshStats();

        // Visual update
        PlayerVisual.DisplayItem(slot, item);
    }

    private Item DropItem(ItemSlot slot) {
        Item dropped = Equipment[slot];
        dropped.Equipped = false;
        Equipment[slot] = null;
        return dropped;
    }

#endregion

    private void RefreshStats() {
        List<PlayerStats> statIndexes = Stats.Keys.ToList();
        foreach (PlayerStats statIdx in statIndexes) {
            Stats[statIdx] = 0;
        }
        foreach (Item item in Equipment.Values) {
            Stats[PlayerStats.Strength] += item.Strength;
            Stats[PlayerStats.MagicPower] += item.Magic;
            Stats[PlayerStats.AttackSpeed] += item.AttackSpeed;
            Stats[PlayerStats.MovementSpeed] += item.MovementSpeed;
            Stats[PlayerStats.Defense] += item.Defense;
            Stats[PlayerStats.MaxHp] += item.MaxHp;
        }
    }

}