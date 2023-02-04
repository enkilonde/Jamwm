using UnityEngine;

public class LootableItem : MonoBehaviour {

    [SerializeField]
    private MeshRenderer _renderer;

    public Item Loot { get; private set; }

    public void Configure(Item lootable) {
        Loot = lootable;

        // TODO : visual configuration of the "Lootable" item
        // (this is just an example)
        _renderer.sharedMaterial.color = Loot.FxColor;
    }

}