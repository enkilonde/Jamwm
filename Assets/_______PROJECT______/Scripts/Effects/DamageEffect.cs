using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageEffect : MonoBehaviour
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private Vector3 _basePosition;
    [SerializeField] private float _basePositionRadius;
    
    [SerializeField] private AnimationCurve _curve;
    [SerializeField] private AnimationCurve _curveOut;
    
    [SerializeField] private Gradient _gradientColor;
    [SerializeField] private AnimationCurve _curveDamageGradient;

    
    public void Init(int amount, Vector3 position)
    {
        float damageScale = _curveDamageGradient.Evaluate(amount);
        transform.position = position+_basePosition;
        _text.text = amount.ToString();
        _text.color = _gradientColor.Evaluate(damageScale);
        _text.transform.localScale =Vector3.zero;
        _text.DOCounter(0, amount, 0.5f);
        Sequence seq = DOTween.Sequence();
        seq.AppendCallback(() =>
        {
            _text.alpha = 0f;
            _text.transform.localPosition = Vector3.up + Random.insideUnitSphere*_basePositionRadius;
            _text.DOFade(1f, 0.2f);
            _text.transform.DOScale(1f, 0.3f).SetEase(Ease.OutBack, 5f*damageScale);
            transform.DOShakePosition(0.5f, damageScale,Mathf.RoundToInt(damageScale*50f));
        });
        seq.AppendInterval(0.5f);
        seq.AppendCallback(() =>
        {
            _text.DOFade(0f, 2.0f).SetEase(Ease.InOutSine);
            _text.transform.DOLocalMoveY(5f, 2f).SetEase(_curveOut).OnComplete(()=>Destroy(gameObject));
        });
        
    }
}
