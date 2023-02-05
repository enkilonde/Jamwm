using System;
using UnityEngine;
using UnityEngine.UI;

public class TempLootUi : MonoBehaviour {

    public static TempLootUi Instance;

    public CanvasGroup CanvasGroup;

    public Text Title;

    public Text LabelSTR;
    public Text LabelMAG;
    public Text LabelDEF;
    public Text LabelSPD;
    public Text LabelMOV;
    public Text LabelHP;

    private Item _currentItem;

    private void Awake() {
        Instance = this;
    }

    public void SetVisible(bool visible) {
        CanvasGroup.alpha = visible ? 1 : 0;
    }

    public void ForgetItem(Item item) {
        if (_currentItem != item) return;

        SetVisible(false);
        _currentItem = null;
        SetVisible(false);
    }

    public void Configure(Item item, Item currentItem = null) {
        Title.text = item.Name;

        int currentStr = currentItem != null ? currentItem.Strength : 0;
        LabelSTR.text = "STR : " + (item.Strength >= currentStr ? "+" : "") + (item.Strength - currentStr);
        if (item.Strength != currentStr) LabelSTR.color = (item.Strength > currentStr) ? Color.green : Color.red;
        else LabelSTR.color = Color.white;

        int currentMag = currentItem != null ? currentItem.Magic : 0;
        LabelMAG.text = "MAG : " + (item.Magic >= 0 ? "+" : "") + (item.Magic - currentMag);
        if (item.Magic != currentMag) LabelMAG.color = (item.Magic > currentMag) ? Color.green : Color.red;
        else LabelMAG.color = Color.white;

        int currentDef = currentItem != null ? currentItem.Defense : 0;
        LabelDEF.text = "DEF : " + (item.Defense >= 0 ? "+" : "") + (item.Defense - currentDef);
        if (item.Defense != currentDef) LabelDEF.color = (item.Defense > currentDef) ? Color.green : Color.red;
        else LabelDEF.color = Color.white;

        int currentSpd = currentItem != null ? currentItem.AttackSpeed : 0;
        LabelSPD.text = "SPD : " + (item.AttackSpeed >= 0 ? "+" : "") + (item.AttackSpeed - currentSpd);
        if (item.AttackSpeed != currentSpd) LabelSPD.color = (item.AttackSpeed > currentSpd) ? Color.green : Color.red;
        else LabelSPD.color = Color.white;

        int currentMov = currentItem != null ? currentItem.MovementSpeed : 0;
        LabelMOV.text = "MOV : " + (item.MovementSpeed >= 0 ? "+" : "") + (item.MovementSpeed - currentMov);
        if (item.MovementSpeed != currentMov) LabelMOV.color = (item.MovementSpeed > currentMov) ? Color.green : Color.red;
        else LabelMOV.color = Color.white;

        int currentHp = currentItem != null ? currentItem.MaxHp : 0;
        LabelHP.text = "HP : " + (item.MaxHp >= 0 ? "+" : "") + (item.MaxHp - currentHp);
        if (item.MaxHp != currentHp) LabelHP.color = (item.MaxHp > currentHp) ? Color.green : Color.red;
        else LabelHP.color = Color.white;

        _currentItem = item;
    }

}