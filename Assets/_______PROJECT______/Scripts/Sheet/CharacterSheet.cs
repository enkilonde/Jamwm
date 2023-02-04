using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CharacterSheet {

    public Dictionary<ItemSlot, Item> Equipment;

    public Dictionary<PlayerStats, int> Stats;
    protected PlayerVisual PlayerVisual;

    private readonly Transform _playerTransform;


    public void Equip(ItemSlot slot, Item item)
    {
        // Optional phase 1 : drop a replaced item
        if (Equipment.ContainsKey(slot))
        {
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

    private Item DropItem(ItemSlot slot)
    {
        Item dropped = Equipment[slot];
        dropped.Equipped = false;
        Equipment[slot] = null;
        return dropped;
    }

    private void RefreshStats()
    {
        List<PlayerStats> statIndexes = Stats.Keys.ToList();
        foreach (PlayerStats statIdx in statIndexes)
        {
            Stats[statIdx] = 0;
        }
        foreach (Item item in Equipment.Values)
        {
            Stats[PlayerStats.Strength] += item.Strength;
            Stats[PlayerStats.MagicPower] += item.Magic;
            Stats[PlayerStats.AttackSpeed] += item.AttackSpeed;
            Stats[PlayerStats.MovementSpeed] += item.MovementSpeed;
            Stats[PlayerStats.Defense] += item.Defense;
            Stats[PlayerStats.MaxHp] += item.MaxHp;
        }
    }

}