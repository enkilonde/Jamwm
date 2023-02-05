using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ElementLoot : MonoBehaviour
{
    public Image Buff;
    public Image Nerf;

    public TextMeshProUGUI StatName;
    public TextMeshProUGUI StatValue;


    public Color ColorBuff;
    public Color ColorNerf;
    public void SetValue(int value)
    {
        print(value);
        DOTween.Kill(gameObject);
        Color color = Color.white;
        
        if (value>0)
        {
            Buff.gameObject.SetActive(true);
            Nerf.gameObject.SetActive(false);
            color = ColorBuff;

            StatValue.DOCounter(0, value, .4f).SetId(gameObject).OnUpdate(() =>
            {
                StatValue.text = "+" + StatValue.text;
            });
        }
        else if (value < 0)
        {
            Buff.gameObject.SetActive(false);
            Nerf.gameObject.SetActive(true);
            color = ColorNerf;
            
            StatValue.DOCounter(0, value, .4f).SetId(gameObject).OnUpdate(() =>
            {
                StatValue.text = StatValue.text;
            });
        }
        else
        {
            Buff.gameObject.SetActive(false);
            Nerf.gameObject.SetActive(false);

            StatValue.text = "-";

        }

        StatName.color = color;
        StatValue.color = color;
    }
}
