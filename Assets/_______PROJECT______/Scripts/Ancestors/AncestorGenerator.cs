using System.Collections.Generic;
using UnityEngine;

public class AncestorGenerator : MonoBehaviour {

    public static AncestorGenerator Instance;

    [SerializeField] private ItemDatabase itemDatabase;

    private void Awake() {
        Instance = this;
    }

#region Level Design

    public (AncestorData, AncestorData) GetInitialParents() {
        return (GenerateAncestor(1), GenerateAncestor(1));
    }

    public (AncestorData, AncestorData) GetParents(AncestorData node) {
        int parentsLevel = node.Level + 1;
        return (GenerateAncestor(parentsLevel), GenerateAncestor(parentsLevel));
    }

    public AncestorData GenerateAncestor(int level) {
        return new AncestorData(
            name: GenerateAncestorName(),
            level: level
        );
    }

#endregion

    public Dictionary<PlayerStats, int> GenerateBossStats(int bossLevel) {
        return new Dictionary<PlayerStats, int>() {
            {PlayerStats.Strength, bossLevel * 2},
            {PlayerStats.MagicPower, bossLevel * 2},
            {PlayerStats.AttackSpeed, bossLevel * 2},
            {PlayerStats.MovementSpeed, bossLevel * 2},
            {PlayerStats.Defense, bossLevel * 2},
            {PlayerStats.MaxHp, bossLevel * 10}
        };
    }

    public Dictionary<ItemSlot, Item> GenerateBossEquipment(int bossLevel) {
        return new Dictionary<ItemSlot, Item>() {
            {ItemSlot.Head, itemDatabase.GetRandomItem(ItemKind.Helmet)},
            {ItemSlot.Torso, itemDatabase.GetRandomItem(ItemKind.Armor)},
            {ItemSlot.LeftArm, itemDatabase.GetRandomItem(ItemKind.Ring)},
            {ItemSlot.RightArm, itemDatabase.GetRandomItem(ItemKind.Ring)},
            {ItemSlot.Ring1, itemDatabase.GetRandomItem(ItemKind.Ring)}
        };
    }

    private string GenerateAncestorName() {
        return "ancestor_" + UnityEngine.Random.Range(0, 1000);
    }

}