using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TransitionUiController : MonoBehaviour {

    [SerializeField] private CanvasGroup _overlay;

    public void Transition(TweenCallback halfwayAction, TweenCallback callbackAction) {
        var anim = DOTween.Sequence();
        anim.Append(_overlay.DOFade(1, 0.15f).SetEase(Ease.Linear));
        anim.AppendCallback(halfwayAction);
        anim.AppendInterval(0.2f);
        anim.Append(_overlay.DOFade(0, 0.15f).SetEase(Ease.Linear));
        anim.onComplete = callbackAction;
    }
    
}