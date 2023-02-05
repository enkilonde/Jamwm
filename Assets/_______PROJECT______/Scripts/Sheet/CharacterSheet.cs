using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CharacterSheet {

    public Dictionary<ItemSlot, Item> Equipment;

    public Dictionary<PlayerStats, int> Stats;
    protected PlayerVisual PlayerVisual;

    protected Transform _characterTransform;

    public int CurrentHp { get; protected set; }
    public int MaxHp => Stats[PlayerStats.MaxHp];
    public float HpRatio => CurrentHp / (float) MaxHp;

    protected CharacterSheet(Transform characterTransform) {
        _characterTransform = characterTransform;
        Equipment = new Dictionary<ItemSlot, Item>();
    }

    public void Equip(ItemID itemID) {
        Item item = LootSpawner.Instance.ItemDatabase.GetItem(itemID);
        Equip(item);
    }

    public void Equip(Item item) {
        Equip(GetSlotFromKind(item.Kind), item);
    }

    public void Equip(ItemSlot slot, Item item) {
        // Optional phase 1 : drop a replaced item
        if (Equipment.ContainsKey(slot)) {
            // Data update
            var droppedItem = DropItem(slot);

            if (droppedItem != null) {
                // Visual update
                PlayerVisual.ClearSlot(slot);
                LootSpawner.Instance.SpawnLoot(droppedItem, _characterTransform.position);
                Debug.Log("Dropping " + droppedItem.name);
            }
        }

        // Main Phase 2 : equip the item

        // Data update
        Item spawnedItem = PlayerVisual.DisplayItem(slot, item);
        if (spawnedItem != null) {
            Equipment[slot] = spawnedItem;
            spawnedItem.Equipped = true;
        }
        RefreshStats();
    }

    private Item DropItem(ItemSlot slot) {
        Item dropped = Equipment[slot];
        if (dropped != null) {
            dropped.Equipped = false;
        }
        Equipment[slot] = null;
        return dropped;
    }

    private void RefreshStats() {
        Stats = GetBaseStats();
        foreach (Item item in Equipment.Values) {
            Stats[PlayerStats.Strength] += item.Strength;
            Stats[PlayerStats.MagicPower] += item.Magic;
            Stats[PlayerStats.AttackSpeed] += item.AttackSpeed;
            Stats[PlayerStats.MovementSpeed] += item.MovementSpeed;
            Stats[PlayerStats.Defense] += item.Defense;
            Stats[PlayerStats.MaxHp] += item.MaxHp;
        }
    }

    protected abstract Dictionary<PlayerStats, int> GetBaseStats();

    public ItemSlot GetSlotFromKind(ItemKind kind, bool forceRight = false) {
        switch (kind) {
            case ItemKind.Helmet:
                return ItemSlot.Head;
            case ItemKind.Armor:
                return ItemSlot.Torso;
            case ItemKind.Ring:
                return ItemSlot.Ring1;
            case ItemKind.Weapon:
                if (EmptySlot(ItemSlot.LeftArm))
                    return ItemSlot.LeftArm;
                else if (EmptySlot(ItemSlot.RightArm))
                    return ItemSlot.RightArm;
                else if (forceRight)
                    return ItemSlot.RightArm;
                else
                    return ItemSlot.LeftArm;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public bool EmptySlot(ItemSlot slot) {
        if (!Equipment.ContainsKey(slot)) return true;
        if (Equipment[slot] == null) return true;
        return false;
    }

#region HP modification

    public void Heal(int hitPoints) {
        CurrentHp = Math.Min(CurrentHp + hitPoints, MaxHp);
    }

    public virtual void Hit(int damages) {
        CurrentHp -= damages;
        if (CurrentHp < 0) CurrentHp = 0;
        EffectManager.Instance.DoDamageEffectOn(damages, _characterTransform.position);
    }

#endregion

}