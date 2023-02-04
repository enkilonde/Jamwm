using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSheet : CharacterSheet {


#region Initialization

    public PlayerSheet(PlayerVisual playerVisual, Transform playerTransform) : base() {
        PlayerVisual = playerVisual;
        _playerTransform = playerTransform;
        Stats = GetInitialStats();
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



#endregion

   

}