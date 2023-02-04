using System.Collections.Generic;

public abstract class CharacterSheet {

    public Dictionary<ItemSlot, Item> Equipment;

    public Dictionary<PlayerStats, int> Stats;
    protected PlayerVisual PlayerVisual;

}