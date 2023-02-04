using UnityEngine;

public class AncestorGenerator : MonoBehaviour {

    public static AncestorGenerator Instance;

    private void Awake() {
        Instance = this;
    }

#region Level Design

    public (AncestorData, AncestorData) GetInitialParents() {
        return (GenerateAncestor(0), GenerateAncestor(0));
    }

    public (AncestorData, AncestorData) GetParents(AncestorData node) {
        int parentsLevel = node.Level + 1;
        return (GenerateAncestor(parentsLevel), GenerateAncestor(parentsLevel));
    }

    private AncestorData GenerateAncestor(int level) {
        return new AncestorData(
            name: GenerateAncestorName(),
            level: level
        );
    }

#endregion

    private string GenerateAncestorName() {
        return "Ancestor_" + UnityEngine.Random.Range(0, 1000);
    }

}