using Sirenix.OdinInspector;
using UnityEngine;

public class CheatsManager : MonoBehaviour {

    [Header("Necessary references")]
    [SerializeField] private CustomCharacterController _player;

    public static float PlayerSpeedModifier = 1;

    [Button]
    public void SetInvincible() {
        ((PlayerSheet) _player.CharacterSheet).Invincible = true;
    }

    [Button]
    public void FullHeal() {
        _player.CharacterSheet.Heal(9999);
    }

    [Button]
    public void ReShuffleNextAncestors() {
        RoomManager.Instance.ReShuffleNextAncestors();
    }

    [Button]
    public void Suicide() {
        _player.CharacterSheet.Hit(9999);
    }

}