using System.Collections.Generic;

public class AncestorData {

    public readonly string Name;
    public readonly int Level;
    public bool InitialRoomAncestor => Level == 0;

    private BossSheet _sheet;

    public Dictionary<PlayerStats, int> stats;
    public Dictionary<ItemSlot, Item> equipment;

    public AncestorData(string name, int level) {
        Name = name;
        Level = level;
        stats = AncestorGenerator.Instance.GenerateBossStats(Level);
        equipment = AncestorGenerator.Instance.GenerateBossEquipment(Level);
    }

    public BossSheet GetDetailedSheet(CustomCharacterController boss) {
        if (_sheet == null) {
            _sheet = new BossSheet(boss, stats, equipment);
        }

        return _sheet;
    }

}