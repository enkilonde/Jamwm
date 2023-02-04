using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class UIWindow : MonoBehaviour
{

    [SerializeField] private RectTransform _rt;
    [SerializeField] private CanvasGroup _cg;
    
    [SerializeField] private Button _exitButton;

    public void Init()
    {
        print("init button");
        _exitButton.onClick.AddListener(() => { CloseWindow(); });
        gameObject.SetActive(false);
        _cg.alpha = 0f;

    }

    public void OpenWindow()
    {
        DOTween.Complete(gameObject);

        gameObject.SetActive(true);
        _cg.alpha = 0f;
        GetComponent<CanvasGroup>().DOFade(1f, 0.2f).SetId(gameObject);
    }

    private void CloseWindow()
    {
        DOTween.Complete(gameObject);
        _cg.alpha = 1f;
        GetComponent<CanvasGroup>().DOFade(0f, 0.2f).SetId(gameObject).OnComplete(() => { gameObject.SetActive(false);});
    }
}
