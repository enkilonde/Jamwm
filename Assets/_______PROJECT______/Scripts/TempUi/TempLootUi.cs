using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TempLootUi : MonoBehaviour {

    public static TempLootUi Instance;

    public CanvasGroup CanvasGroup;

    public TextMeshProUGUI Title;
    public TextMeshProUGUI OldTitle;

    public ElementLoot str;
    public ElementLoot mag;
    public ElementLoot def;
    public ElementLoot spd;
    public ElementLoot mov;
    public ElementLoot hp;

    public Item CurrentItem { get; private set; }

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (PlayerItemPicker.Instance.Lootable != null) {


                PlayerItemPicker.Instance.ValidateLoot(false);
            }
        }
    }

    public void PickupLeft()
    {
        if (PlayerItemPicker.Instance.Lootable == null) return;
        PlayerItemPicker.Instance.ValidateLoot(false);
    }

    public void PickupRight()
    {
        if (PlayerItemPicker.Instance.Lootable == null) return;
        PlayerItemPicker.Instance.ValidateLoot(true);
    }

    public void SetVisible(bool visible)
    {
        print("set visible");
        DOTween.Kill(gameObject);
        CanvasGroup.DOFade(visible ? 1 : 0, 0.5f).SetId(gameObject); 
    }

    public void ForgetItem(Item item) {
        if (CurrentItem != item) return;

        SetVisible(false);
        CurrentItem = null;
        SetVisible(false);
    }

    public void Configure(Item item, Item currentItem = null) {

        if (currentItem!=null)
        {
            OldTitle.gameObject.SetActive(true);
            OldTitle.text = currentItem.Name;
        }
        else
        {
            OldTitle.gameObject.SetActive(false);
        }

        Title.text = item.Name;
        
        
        print(item.Strength);
        int currentStr = currentItem != null ? currentItem.Strength : 0;

        print(currentStr);
        //
        str.SetValue( item.Strength - currentStr);
        int currentMag = currentItem != null ? currentItem.Magic : 0;
        mag.SetValue( item.Magic - currentMag);
        int currentDef = currentItem != null ? currentItem.Defense : 0;
        def.SetValue( item.Defense - currentDef);
        int currentAttackSpeed = currentItem != null ? currentItem.AttackSpeed : 0;
        spd.SetValue( item.AttackSpeed - currentAttackSpeed);
        int currentSpeed = currentItem != null ? currentItem.MovementSpeed : 0;
        mov.SetValue( item.MovementSpeed - currentSpeed);
        int currentHP = currentItem != null ? currentItem.MaxHp : 0;
        hp.SetValue( item.MaxHp - currentHP);

        // LabelSTR.text = "STR : " + (item.Strength >= currentStr ? "+" : "") + (item.Strength - currentStr);
        // if (item.Strength != currentStr) LabelSTR.color = (item.Strength > currentStr) ? Color.green : Color.red;
        // else LabelSTR.color = Color.white;
        //
        // int currentMag = currentItem != null ? currentItem.Magic : 0;
        // LabelMAG.text = "MAG : " + (item.Magic >= currentMag ? "+" : "") + (item.Magic - currentMag);
        // if (item.Magic != currentMag) LabelMAG.color = (item.Magic > currentMag) ? Color.green : Color.red;
        // else LabelMAG.color = Color.white;
        //
        // int currentDef = currentItem != null ? currentItem.Defense : 0;
        // LabelDEF.text = "DEF : " + (item.Defense >= currentDef ? "+" : "") + (item.Defense - currentDef);
        // if (item.Defense != currentDef) LabelDEF.color = (item.Defense > currentDef) ? Color.green : Color.red;
        // else LabelDEF.color = Color.white;
        //
        // int currentSpd = currentItem != null ? currentItem.AttackSpeed : 0;
        // LabelSPD.text = "SPD : " + (item.AttackSpeed >= currentSpd ? "+" : "") + (item.AttackSpeed - currentSpd);
        // if (item.AttackSpeed != currentSpd) LabelSPD.color = (item.AttackSpeed > currentSpd) ? Color.green : Color.red;
        // else LabelSPD.color = Color.white;
        //
        // int currentMov = currentItem != null ? currentItem.MovementSpeed : 0;
        // LabelMOV.text = "MOV : " + (item.MovementSpeed >= currentMov ? "+" : "") + (item.MovementSpeed - currentMov);
        // if (item.MovementSpeed != currentMov)
        //     LabelMOV.color = (item.MovementSpeed > currentMov) ? Color.green : Color.red;
        // else LabelMOV.color = Color.white;
        //
        // int currentHp = currentItem != null ? currentItem.MaxHp : 0;
        // LabelHP.text = "HP : " + (item.MaxHp >= currentHp ? "+" : "") + (item.MaxHp - currentHp);
        // if (item.MaxHp != currentHp) LabelHP.color = (item.MaxHp > currentHp) ? Color.green : Color.red;
        // else LabelHP.color = Color.white;

        CurrentItem = item;
    }

}