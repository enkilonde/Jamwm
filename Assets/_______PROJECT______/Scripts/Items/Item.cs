using UnityEngine;

public class Item : MonoBehaviour {

    [Header("Item Metadata")]
    public string Name;
    public ItemID ID;
    public ItemKind Kind;
    public Color FxColor;

    [Header("Stats Modifiers")]
    public int Strength;
    public int Magic;
    public int AttackSpeed;
    public int MovementSpeed;
    public int Defense;
    public int MaxHp;

    // Item State
    public bool Equipped { get; set; }

}