using System;
using UnityEngine;

public class GameOverManager : MonoBehaviour {

    public static GameOverManager Instance;

    private void Awake() {
        Instance = this;
    }

    public void TriggerGameOver() {
        // TODO
    }
    
}