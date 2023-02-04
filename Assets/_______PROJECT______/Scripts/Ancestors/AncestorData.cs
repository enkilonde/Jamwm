public class AncestorData {

    public readonly string Name;
    public readonly int Level;
    public bool InitialRoomAncestor => Level == 0;

    public AncestorData(string name, int level) {
        Name = name;
        Level = level;
    }

}