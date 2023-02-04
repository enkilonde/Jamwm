using System;
using System.Collections.Generic;

[Serializable]
public class Item {

    public ItemID ID;
    public string Name { get; }
    public ItemKind Kind { get; }
    public Dictionary<PlayerStats, int> Modifiers { get; }

}