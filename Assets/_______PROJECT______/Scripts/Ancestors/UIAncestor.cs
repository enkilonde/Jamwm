using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIAncestor : MonoBehaviour
{
    public TextMeshProUGUI Name;
    public Image Portrait;

    private int _depth;

    [SerializeField] private RectTransform _rt;
    // Start is called before the first frame update
    public void Setup(string name, Sprite portrait, int depth)
    {
        Name.text = name;
        Portrait.sprite = portrait;
        _depth = depth;
        //_rt.anchoredPosition = new
        
    }
}
