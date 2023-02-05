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
            {PlayerStats.Strength, 100 + bossLevel * 20},
            {PlayerStats.MagicPower, 50 + bossLevel * 10},
            {PlayerStats.AttackSpeed, 100 + bossLevel * 20},
            {PlayerStats.MovementSpeed, 100 + bossLevel * 20},
            {PlayerStats.Defense, 20 + bossLevel * 5},
            {PlayerStats.MaxHp, 1000 + bossLevel * 100}
        };
    }

    public Dictionary<ItemSlot, Item> GenerateBossEquipment(int bossLevel) {
        return new Dictionary<ItemSlot, Item>() {
            {ItemSlot.Head, itemDatabase.GetRandomItem(ItemKind.Helmet)},
            {ItemSlot.Torso, itemDatabase.GetRandomItem(ItemKind.Armor)},
            {ItemSlot.LeftArm, itemDatabase.GetRandomItem(ItemKind.Weapon)},
            {ItemSlot.RightArm, itemDatabase.GetRandomItem(ItemKind.Weapon)},
            {ItemSlot.Ring1, itemDatabase.GetRandomItem(ItemKind.Ring)}
        };
    }

    private string GenerateAncestorName() {
        return "ancestor_" + UnityEngine.Random.Range(0, 1000);
    }

}