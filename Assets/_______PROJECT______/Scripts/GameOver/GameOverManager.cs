using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        
        RoomManager.Instance._playerTransform.gameObject.SetActive(false);
        RoomManager.Instance._currentRoom.BossRef.gameObject.SetActive(false);

        StartCoroutine(WaitAndRelaunchGame());
    }

    private IEnumerator WaitAndRelaunchGame() {
        yield return new WaitForSeconds(3);
        _gameOverUi.FadeOut();
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("MenuScene");
    }

}