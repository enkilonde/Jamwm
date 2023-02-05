using System;
using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour {

    public abstract ItemKind Kind { get; }


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

    private bool _equipped;
    [SerializeField]private ParticleSystem _fx;
    // Item State
    public bool Equipped
    {
        get=>_equipped;
        set
        {
            _equipped = value;
            if (_equipped)
            {
                _fx.Stop();
            }
            else
            {
                _fx.Play();
            }
        }
    }
}