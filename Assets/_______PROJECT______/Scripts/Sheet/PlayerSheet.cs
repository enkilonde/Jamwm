using System.Collections.Generic;

public class PlayerSheet {

    public Dictionary<PlayerStats, int> Stats;
    public Dictionary<ItemSlot, Item> Equipment;

    public PlayerSheet() {
        Stats = new Dictionary<PlayerStats, int>();
        Equipment = new Dictionary<ItemSlot, Item>();
    }

    public void Equip(ItemSlot slot, Item item) {
        if (Equipment.ContainsKey(slot)) {
            var droppedItem = DropItem(slot);
            // TODO : spawn dropped item on ground
        }

        Equipment[slot] = item;

        RefreshStats();

        // TODO : visual model refresh
    }

    private Item DropItem(ItemSlot slot) {
        Item dropped = Equipment[slot];
        Equipment[slot] = null;
        return dropped;
    }

    private void RefreshStats() {
        foreach (var kvp in Stats) {
            Stats[kvp.Key] = 0;
        }
        foreach (var kvp in Equipment) {
            Item item = kvp.Value;
            foreach (var modifier in item.Modifiers) {
                Stats[modifier.Key] += modifier.Value;
            }
        }
    }

}