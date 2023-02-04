using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour {

    public static GameOverManager Instance;

    [SerializeField] private GameOverUiController _gameOverUi;

    private void Awake() {
        Instance = this;
    }

    public void TriggerGameOver() {
        List<AncestorData> defeatedAncestors = new List<AncestorData>(SaveManager.Instance.DefeatedAncestors);
        SaveManager.Instance.ClearCurrentData();

        _gameOverUi.Appear(defeatedAncestors);
    }

}