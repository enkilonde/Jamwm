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
            {PlayerStats.Strength, 100},
            {PlayerStats.MagicPower, 50},
            {PlayerStats.AttackSpeed, 100},
            {PlayerStats.MovementSpeed, 100},
            {PlayerStats.Defense, 20},
            {PlayerStats.MaxHp, 1000}
        };
    }

#endregion

    protected override void RefreshStats() {
        base.RefreshStats();

        LazyUiHook.Instance.PlayerLifeBar.UpdateMaxHp(Stats[PlayerStats.MaxHp]);
    }

    public override void Hit(int damages) {
        if (Invincible) return;

        base.Hit(damages);

        LazyUiHook.Instance.PlayerLifeBar.SetPlayerHP(CurrentHp);

        if (CurrentHp <= 0) {
            GameOverManager.Instance.TriggerGameOver();
        }
    }

}