using System.Collections.Generic;
using System.Linq;

public class BossSheet : CharacterSheet {

    private readonly Dictionary<PlayerStats, int> _baseStats;

    public BossSheet(
        PlayerVisual bossVisualPlayer,
        Dictionary<PlayerStats, int> stats,
        Dictionary<ItemSlot, Item> equipment
    ) : base(bossVisualPlayer.transform.parent) {
        _baseStats = stats;

        base.PlayerVisual = bossVisualPlayer;
        base.Equipment = equipment;
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

    public List<Item> BossLoots() {
        return Equipment.Values.ToList();
    }
    
}