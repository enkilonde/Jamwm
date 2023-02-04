public class AncestorData {

    public readonly string Name;
    public readonly int Level;
    public bool InitialRoomAncestor => Level == 0;
   
    private BossSheet _sheet;

    public AncestorData(string name, int level) {
        Name = name;
        Level = level;
    }

    public BossSheet GetDetailedSheet() {
        if (_sheet == null) {
            _sheet = new BossSheet(
                stats:AncestorGenerator.GenerateBossStats(Level)
            );
        }

        return _sheet;
    }

}