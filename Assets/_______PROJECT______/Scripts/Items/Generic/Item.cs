using System;
using UnityEngine;

[Serializable]
public abstract class Item {

    public abstract ItemKind Kind { get; }

    [Header("Item Metadata")]
    public string Name;
    public ItemID ID;
    public Color FxColor;

    [Header("Item Visual")]
    public GameObject EquippedPrefab;

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