using UnityEngine;

public class PauseManager : MonoBehaviour {

    [SerializeField]
    private GameObject PauseUI;

    private bool _isPaused;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            _isPaused = !_isPaused;
            Time.timeScale = _isPaused ? 0 : 1;
            PauseUI.SetActive(_isPaused);
        }
    }

}