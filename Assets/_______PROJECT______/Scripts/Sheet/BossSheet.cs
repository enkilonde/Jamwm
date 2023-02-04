using System.Collections.Generic;

public class BossSheet : CharacterSheet {

    private readonly Dictionary<PlayerStats, int> _baseStats;

    public BossSheet(PlayerVisual bossVisualPlayer, Dictionary<PlayerStats, int> stats) : base(bossVisualPlayer.transform.parent) {
        _baseStats = stats;

        base.PlayerVisual = bossVisualPlayer;
        base.Stats = _baseStats;
        CurrentHp = MaxHp;
    }

    protected override Dictionary<PlayerStats, int> GetBaseStats() {
        return _baseStats;
    }

    public override void Hit(int damages) {
        base.Hit(damages);

        LazyUiHook.Instance.BossLifeBar.SetValue(HpRatio);
        
        if (CurrentHp <= 0) {
            RoomManager.Instance.HandleBossHealthZero();
        }
    }
    
}