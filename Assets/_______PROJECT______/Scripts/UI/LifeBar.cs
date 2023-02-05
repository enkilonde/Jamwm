using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _HPText;

    [SerializeField]private Slider _slider1;
    [SerializeField]private Slider _slider2;

    [SerializeField]private CanvasGroup _canvasGroup;

    [SerializeField] private bool _isPlayerBar;

    [SerializeField,ShowIf("_isPlayerBar")]private Image _playerAvatar;
    [SerializeField,ShowIf("_isPlayerBar")]private Image _playerWarning;

    [ShowIf("_isPlayerBar")]private int _playerMaxHP;
    [ShowIf("_isPlayerBar")]private int _currentPlayerHP;
    [ShowIf("_isPlayerBar")]private int _playerHP;

    private bool _warningActive;

    private void Update()
    {
        if (_isPlayerBar)
        {
            if (_slider1.value<0.2 && _warningActive==false)
            {
                _playerWarning.DOFade(1f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetId(_playerWarning);
                _warningActive = true;
            }
            else if (_slider1.value>0.2 && _warningActive==true)
            {
                DOTween.Kill(_playerWarning);
                _playerWarning.DOFade(0f, 0.5f);
                _warningActive = false;
            }
        }
    }

    public void SetBossName(string bossName) {
        if (_isPlayerBar) return;

        _HPText.text = bossName;
    }

    public void FadeTo(bool visible, TweenCallback callback) {
        Tween fade = _canvasGroup.DOFade(visible ? 1 : 0, 0.3f);
        
        if (callback != null)
            fade.onComplete = callback;
    }

    [Button]
    public void SetValue(float value)
    {
        if (value<_slider1.value && !_isPlayerBar)
        {
            DOTween.Kill(_HPText);

            _HPText.transform.DOPunchScale(Vector3.one*-0.2f, 0.5f);
            _HPText.DOColor(Color.red, 0.1f).SetId(_HPText).OnComplete(() =>
            {
                _HPText.DOColor(Color.white, 0.5f).SetId(_HPText);
            });
        }
        DOTween.Kill(gameObject);
        Sequence seq = DOTween.Sequence();
        seq.SetId(gameObject);
        seq.Append(_slider1.DOValue(value, 0.5f));
        seq.Append(_slider2.DOValue(value, 1f));
    }

    [Button,ShowIf("_isPlayerBar")]
    public void InitPlayerBar(int maxHP)
    {
        _playerHP = maxHP;
        _playerMaxHP = maxHP;
        _currentPlayerHP = maxHP;
        UpdatePlayerText();
    }

    public void UpdateMaxHp(int maxHP)
    {
        _playerMaxHP = maxHP;

        // Security in case you drop an HP-boosting item
        if (_currentPlayerHP > _playerMaxHP) {
            _currentPlayerHP = _playerMaxHP;
        }
        if (_playerHP > _playerMaxHP) {
            _playerHP = _playerMaxHP;
        }

        UpdatePlayerText();
    }
    
    [Button,ShowIf("_isPlayerBar")]
    public void SetPlayerHP(int hp)
    {
        if (!_isPlayerBar) return;
        
        DOTween.Kill(_HPText);
        if (hp<_playerHP)
        {
            _playerAvatar.transform.DOPunchScale(Vector3.one*-0.2f, 0.5f);
            _playerAvatar.DOColor(Color.red, 0.1f).SetId(_HPText).OnComplete(() =>
            {
                _playerAvatar.DOColor(Color.white, 0.5f).SetId(_HPText);
            });
        }
        else if (hp> _playerHP)
        {
            _playerAvatar.transform.DOPunchScale(Vector3.one*0.2f, 2f);

            _playerAvatar.DOColor(Color.green, 0.2f).SetId(_HPText).OnComplete(() =>
            {
                _playerAvatar.DOColor(Color.white, 0.5f).SetId(_HPText);
            });

        }

        _playerHP = hp;
        SetValue(_playerHP/(float)_playerMaxHP);
        UpdatePlayerText();
    }

    private void UpdatePlayerText()
    {
        if (!_isPlayerBar) return;

        _HPText.DOCounter(_currentPlayerHP, _playerHP, 0.5f).OnUpdate(() =>
        {
            var text = _HPText.text;
            _HPText.text = "<size=80%>HP<size=100%> <color=#"+(_warningActive?"FF0000>":"FFFFFF>") + text + " <color=#AAAAAA>/" + _playerMaxHP.ToString();

        }).OnComplete(() =>
        {
            _currentPlayerHP = _playerHP;
        });
    }
    
    [Button]
    private void SetTo5()
    {
        SetPlayerHP(5);
    }
    [Button]
    private void SetTo50()
    {
        SetPlayerHP(50);

    }
    [Button]
    private void SetTo180()

    {
        SetPlayerHP(180);

    }
    
    [Button]
    private void SetTo200()

    {
        SetPlayerHP(200);

    }
    [Button]
    private void SetTo250()

    {
        SetPlayerHP(250);

    }


}
