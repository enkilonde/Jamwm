using System.Collections.Generic;
using UnityEngine;

public class PlayerSheet : CharacterSheet {

    public bool Invincible;

#region Initialization

    public PlayerSheet(PlayerVisual playerVisual, Transform playerTransform) : base(playerTransform) {
        PlayerVisual = playerVisual;
        Stats = GetBaseStats();
        CurrentHp = Stats[PlayerStats.MaxHp];
    }

    protected override Dictionary<PlayerStats, int> GetBaseStats() {
        return new Dictionary<PlayerStats, int> {
            {PlayerStats.Strength, 1},
            {PlayerStats.MagicPower, 1},
            {PlayerStats.AttackSpeed, 1},
            {PlayerStats.MovementSpeed, 1},
            {PlayerStats.Defense, 1},
            {PlayerStats.MaxHp, 10}
        };
    }

#endregion

#region Inventory

    public void PickUp(LootableItem lootable) {
        var itemKind = lootable.Loot.Kind;
        var slot = GetSlotFromKind(itemKind);
        Equip(slot, lootable.Loot);
    }

#endregion

    public override void Hit(int damages) {
        if (Invincible) return;

        base.Hit(damages);

        LazyUiHook.Instance.PlayerLifeBar.SetPlayerHP(CurrentHp);

        if (CurrentHp <= 0) {
            GameOverManager.Instance.TriggerGameOver();
        }
    }

}