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

    public Item CurrentItem { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (PlayerItemPicker.Instance.Lootable != null) {
                PlayerItemPicker.Instance.ValidateLoot();
            }
        }
    }

    public void SetVisible(bool visible) {
        CanvasGroup.alpha = visible ? 1 : 0;
    }

    public void ForgetItem(Item item) {
        if (CurrentItem != item) return;

        SetVisible(false);
        CurrentItem = null;
        SetVisible(false);
    }

    public void Configure(Item item, Item currentItem = null) {
        Title.text = item.Name;

        int currentStr = currentItem != null ? currentItem.Strength : 0;
        LabelSTR.text = "STR : " + (item.Strength >= currentStr ? "+" : "") + (item.Strength - currentStr);
        if (item.Strength != currentStr) LabelSTR.color = (item.Strength > currentStr) ? Color.green : Color.red;
        else LabelSTR.color = Color.white;

        int currentMag = currentItem != null ? currentItem.Magic : 0;
        LabelMAG.text = "MAG : " + (item.Magic >= currentMag ? "+" : "") + (item.Magic - currentMag);
        if (item.Magic != currentMag) LabelMAG.color = (item.Magic > currentMag) ? Color.green : Color.red;
        else LabelMAG.color = Color.white;

        int currentDef = currentItem != null ? currentItem.Defense : 0;
        LabelDEF.text = "DEF : " + (item.Defense >= currentDef ? "+" : "") + (item.Defense - currentDef);
        if (item.Defense != currentDef) LabelDEF.color = (item.Defense > currentDef) ? Color.green : Color.red;
        else LabelDEF.color = Color.white;

        int currentSpd = currentItem != null ? currentItem.AttackSpeed : 0;
        LabelSPD.text = "SPD : " + (item.AttackSpeed >= currentSpd ? "+" : "") + (item.AttackSpeed - currentSpd);
        if (item.AttackSpeed != currentSpd) LabelSPD.color = (item.AttackSpeed > currentSpd) ? Color.green : Color.red;
        else LabelSPD.color = Color.white;

        int currentMov = currentItem != null ? currentItem.MovementSpeed : 0;
        LabelMOV.text = "MOV : " + (item.MovementSpeed >= currentMov ? "+" : "") + (item.MovementSpeed - currentMov);
        if (item.MovementSpeed != currentMov)
            LabelMOV.color = (item.MovementSpeed > currentMov) ? Color.green : Color.red;
        else LabelMOV.color = Color.white;

        int currentHp = currentItem != null ? currentItem.MaxHp : 0;
        LabelHP.text = "HP : " + (item.MaxHp >= currentHp ? "+" : "") + (item.MaxHp - currentHp);
        if (item.MaxHp != currentHp) LabelHP.color = (item.MaxHp > currentHp) ? Color.green : Color.red;
        else LabelHP.color = Color.white;

        CurrentItem = item;
    }

}