using System.Collections.Generic;

public class PlayerSheet {

    public Dictionary<PlayerStats, int> Stats;
    public Dictionary<ItemSlot, Item> Equipment;

#region Initialization

    public PlayerSheet() {
        Stats = GetDefaultStats();
        Equipment = new Dictionary<ItemSlot, Item>();
    }

    private Dictionary<PlayerStats, int> GetDefaultStats() {
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

    public void Equip(ItemSlot slot, Item item) {
        if (Equipment.ContainsKey(slot)) {
            var droppedItem = DropItem(slot);
            // TODO : spawn dropped item on ground
        }

        Equipment[slot] = item;
        item.Equipped = true;

        RefreshStats();

        // TODO : visual model refresh
    }

    private Item DropItem(ItemSlot slot) {
        Item dropped = Equipment[slot];
        dropped.Equipped = false;
        Equipment[slot] = null;
        return dropped;
    }

#endregion

    private void RefreshStats() {
        foreach (var kvp in Stats) {
            Stats[kvp.Key] = 0;
        }
        foreach (var kvp in Equipment) {
            Item item = kvp.Value;
            Stats[PlayerStats.Strength] += item.Strength;
            Stats[PlayerStats.MagicPower] += item.Magic;
            Stats[PlayerStats.AttackSpeed] += item.AttackSpeed;
            Stats[PlayerStats.MovementSpeed] += item.MovementSpeed;
            Stats[PlayerStats.Defense] += item.Defense;
            Stats[PlayerStats.MaxHp] += item.MaxHp;
        }
    }

}