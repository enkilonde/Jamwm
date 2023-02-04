using System.Collections.Generic;

public class BossSheet : CharacterSheet {

    public BossSheet(PlayerVisual bossVisualPlayer, Dictionary<PlayerStats, int> stats) : base(bossVisualPlayer.transform.parent) {
        base.PlayerVisual = bossVisualPlayer;
        base.Stats = stats;
    }

    public override void Hit(int damages) {
        base.Hit(damages);
        if (CurrentHp <= 0) {
            RoomManager.Instance.HandleBossDeath();
            SaveManager.Instance.HandleDefeatedAncestor(RoomManager.Instance._currentRoom.Ancestor);
        }
    }
    
}