using System.Collections.Generic;

public class BossSheet : CharacterSheet {

    public BossSheet(PlayerVisual bossVisualPlayer, Dictionary<PlayerStats, int> stats) : base() {
        base.PlayerVisual = bossVisualPlayer;
        base.Stats = stats;
    }
    
}