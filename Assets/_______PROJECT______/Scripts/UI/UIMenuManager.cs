using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] private UIWindow _highScoresWindow;
    [SerializeField] private UIWindow _creditsWindow;
    
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _creditsButton;
    [SerializeField] private Button _highScoresButton;
    [SerializeField] private Button _quitButton;
    private void Start()
    {
        _highScoresWindow.Init();
        _creditsWindow.Init();
        
        _playButton.onClick.AddListener(Play);
        _quitButton.onClick.AddListener(Quit);

        _highScoresButton.onClick.AddListener(_highScoresWindow.OpenWindow);
        _creditsButton.onClick.AddListener(_creditsWindow.OpenWindow);
    }

    public void Play()
    {
        SceneManager.LoadScene("Crypt", LoadSceneMode.Single);
    }
    public void Quit()
    {
        Application.Quit();
    }
}
