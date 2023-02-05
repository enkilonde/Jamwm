using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public abstract class Item : MonoBehaviour {

    public abstract ItemKind Kind { get; }


    [Header("Item Metadata")]
    public string Name;
    public ItemID ID;

    [ReadOnly] public int Strength;
    [ReadOnly] public int Magic;
    [ReadOnly] public int AttackSpeed;
    [ReadOnly] public int MovementSpeed;
    [ReadOnly] public int Defense;
    [ReadOnly] public int MaxHp;

    [Header("Stats Modifiers")]
    public Vector2Int _Strength;
    public Vector2Int _Magic;
    public Vector2Int _AttackSpeed;
    public Vector2Int _MovementSpeed;
    public Vector2Int _Defense;
    public Vector2Int _MaxHp;

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

    internal void Init()
    {
        Strength = Random.Range(_Strength.x, _Strength.y);
        Magic = Random.Range(_Magic.x, _Magic.y);
        AttackSpeed = Random.Range(_AttackSpeed.x, _AttackSpeed.y);
        MovementSpeed = Random.Range(_MovementSpeed.x, _MovementSpeed.y);
        Defense = Random.Range(_Defense.x, _Defense.y);
        MaxHp = Random.Range(_MaxHp.x, _MaxHp.y);
    }
}