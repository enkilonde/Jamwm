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

    [SerializeField] private RectTransform _logo;
    [SerializeField] private RectTransform _logoDetail;
    
    [SerializeField] private RectTransform _cursor;
    [SerializeField] private Image _cursorGlow;

    private float _targetCursor;

    private void Start()
    {
        _highScoresWindow.Init();
        _creditsWindow.Init();
        
       // _playButton.OnPointerEnter(() => { Play();});
        
        _playButton.onClick.AddListener(Play);
        _quitButton.onClick.AddListener(Quit);

        _highScoresButton.onClick.AddListener(_highScoresWindow.OpenWindow);
        _creditsButton.onClick.AddListener(_creditsWindow.OpenWindow);

        _logo.localEulerAngles = new Vector3(0f, 0f, -5f);
        _logo.DORotate(new Vector3(0f, 0f, 10f), 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        _cursorGlow.DOFade(.2f,1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);

        _targetCursor = _playButton.GetComponent<RectTransform>().anchoredPosition.y;
    }

    private void Update()
    {
        _cursor.anchoredPosition = Vector2.Lerp(_cursor.anchoredPosition,  new Vector2(0f, _targetCursor), 0.1f);
    }

    public void SelectButton(Button button)
    {
        DOTween.Complete(button);
        button.GetComponent<RectTransform>().DOShakeScale(0.2f, -0.2f).SetId(button);
        _targetCursor = button.GetComponent<RectTransform>().anchoredPosition.y;
        
        _logoDetail.DOShakeAnchorPos(0.3f, 3f);

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
