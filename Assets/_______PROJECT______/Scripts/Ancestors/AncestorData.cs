public class AncestorData {

    public readonly string Name;
    public readonly int Level;
    public bool InitialRoomAncestor => Level == 0;

    private BossSheet _sheet;

    public AncestorData(string name, int level) {
        Name = name;
        Level = level;
    }

    public BossSheet GetDetailedSheet(CustomCharacterController boss) {
        if (_sheet == null) {
            _sheet = new BossSheet(
                boss : boss,
                stats: AncestorGenerator.Instance.GenerateBossStats(Level),
                equipment: AncestorGenerator.Instance.GenerateBossEquipment(Level)
            );
        }

        return _sheet;
    }

}