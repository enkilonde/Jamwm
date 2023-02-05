using System.Collections.Generic;
using Coffee.UIEffects;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameOverUiController : MonoBehaviour {

    [SerializeField] private CanvasGroup _group;
    [SerializeField] private RawImage _fadeToBlack;


    [SerializeField] private AnimationCurve _valueCurve;
    [SerializeField] private UIHsvModifier _hsvModifier;
    [SerializeField] private float _speed;
    [SerializeField] private Vector2 _valueRange;
    
    [SerializeField] private List<string> _messages;
    [SerializeField] private TextMeshProUGUI _text;
    private void Update()
    {
        _hsvModifier.value = Mathf.Lerp(_valueRange.x, _valueRange.y,_valueCurve.Evaluate(Time.time*_speed));
    }

    [Button]
    public void Appear(List<AncestorData> victories) {
        _fadeToBlack.color = Color.clear;

        _text.text = "";
        _text.color = new Color(1f,1f,1f,0f);
        
        var gameOverFadeIn = DOTween.Sequence();
        gameOverFadeIn.Append(_group.DOFade(1, 0.3f));
        gameOverFadeIn.Append(_text.DOFade(1, 1f));
        gameOverFadeIn.Join(_text.DOText(_messages[Random.Range(0, _messages.Count - 1)], 1f));
        
    }

    public void FadeOut() {
        _fadeToBlack.DOFade(1, 0.5f);
    }

}