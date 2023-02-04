using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class GameOverUiController : MonoBehaviour {

    [SerializeField] private CanvasGroup _group;

    public void Appear(List<AncestorData> victories) {
        var gameOverFadeIn = DOTween.Sequence();
        gameOverFadeIn.Append(_group.DOFade(1, 0.3f));
    }

}