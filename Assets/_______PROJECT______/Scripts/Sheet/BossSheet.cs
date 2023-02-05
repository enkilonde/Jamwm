using System.Collections.Generic;
using System.Linq;

public class BossSheet : CharacterSheet {

    private readonly Dictionary<PlayerStats, int> _baseStats;

    public BossSheet(
        CustomCharacterController boss,
        Dictionary<PlayerStats, int> stats,
        Dictionary<ItemSlot, Item> equipment
    ) : base(boss) {
        _baseStats = stats;

        base.PlayerVisual = boss.playerVisual;
        base.Stats = _baseStats;
        CurrentHp = MaxHp;

        foreach (var itemModel in equipment.Values) {
            EquipFromItemModel(itemModel);
        }
    }

    protected override Dictionary<PlayerStats, int> GetBaseStats() {
        return _baseStats;
    }

    public override void Hit(int damages) {
        
        SoundManager.INSTANCE.PlaySound(SoundInfo.SoundType.HitEnemy);

        bool alive = CurrentHp > 0;
        base.Hit(damages);

        LazyUiHook.Instance.BossLifeBar.SetValue(HpRatio);
        
        if (alive && CurrentHp <= 0) {
            RoomManager.Instance.HandleBossHealthZero();
            SoundManager.INSTANCE.PlaySound(SoundInfo.SoundType.DeathEnemy);

        }
    }

    public List<Item> BossLoots() {
        return Equipment.Values.ToList();
    }
    
}