using System.Collections.Generic;

public class BossSheet : CharacterSheet {

    public BossSheet(Dictionary<PlayerStats, int> stats) {
        base.Stats = stats;
    }
    
}