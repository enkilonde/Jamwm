using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
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

    [SerializeField] private List<Button> _buttons;

    private float _targetCursor;
    private int _currentButton =0;
    private bool _changedButton =false;
    private float _vertical;

    private void Start()
    {
        _highScoresWindow.Init();
        _creditsWindow.Init();
        SoundManager.INSTANCE.SetMusicType(SoundManager.MusicType.Home);
        SoundManager.INSTANCE.SetMusicState(SoundManager.MusicState.Playing);
   
        _playButton.onClick.AddListener(Play);
        _quitButton.onClick.AddListener(Quit);

        _highScoresButton.onClick.AddListener(_highScoresWindow.OpenWindow);
        _creditsButton.onClick.AddListener(_creditsWindow.OpenWindow);

        _logo.localEulerAngles = new Vector3(0f, 0f, -5f);
        _logo.DORotate(new Vector3(0f, 0f, 10f), 5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);

        _cursorGlow.DOFade(.2f,1f).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
        
        SelectButton(_buttons[_currentButton]);
    }
    
    public void Move(InputAction.CallbackContext context)
    {
        _vertical = context.ReadValue<Vector2>().y;
    }

    private void Update()
    {
        _cursor.anchoredPosition = Vector2.Lerp(_cursor.anchoredPosition,  new Vector2(0f, _targetCursor), 0.3f);
        
        if (_vertical > 0.2f && _changedButton == false)
        {
            _currentButton--;
            if (_currentButton<0)
            {
                _currentButton = _buttons.Count - 1;
            }
            _changedButton = true;
            SelectButton(_buttons[_currentButton]);
        }
        else if (_vertical < -0.2f && _changedButton == false)
        {
            _currentButton++;
            if (_currentButton>=_buttons.Count)
            {
                _currentButton = 0;
            }
            _changedButton = true;
            SelectButton(_buttons[_currentButton]);
        }
        else if (Mathf.Abs(_vertical)<0.2f)
        {
            if (_changedButton)
            {
                _changedButton = false;
            }
        }
    }

    public void SelectButton(Button button)
    {
        SoundManager.INSTANCE.PlaySound(SoundInfo.SoundType.ButtonUIScroll);
        DOTween.Complete(button);
        button.GetComponent<RectTransform>().DOShakeScale(0.2f, -0.2f).SetId(button);
        _targetCursor = button.GetComponent<RectTransform>().anchoredPosition.y;
        
        _logoDetail.DOShakeAnchorPos(0.3f, 3f);
    }
    
    
    public void Play()
    {
        SoundManager.INSTANCE.PlaySound(SoundInfo.SoundType.ButtonUITapIn);

        SoundManager.INSTANCE.SetMusicType(SoundManager.MusicType.Battle);

        SceneManager.LoadScene("Crypt", LoadSceneMode.Single);
    }
    public void Quit()
    {
        Application.Quit();
    }

    public void Select()
    {
        _buttons[_currentButton].onClick.Invoke();
}
}
