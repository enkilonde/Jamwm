using System;
using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour {

    public abstract ItemKind Kind { get; }

    public ParticleSystem chargeFX;

    public ProjectileBehaviour projectilePrefab;
    public float projectileDelay;

    [Header("Item Metadata")]
    public string Name;
    public ItemID ID;

    [Header("Stats Modifiers")]
    public int Strength;
    public int Magic;
    public int AttackSpeed;
    public int MovementSpeed;
    public int Defense;
    public int MaxHp;

    [Header("Model Components")]
    public BoxCollider LootCollider;

    // Item State
    public bool Equipped { get; set; }


}