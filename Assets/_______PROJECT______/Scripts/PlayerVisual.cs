using UnityEngine;

public class PlayerVisual : MonoBehaviour {

    public ParticleSystem dashFX;

    [Header("Body Parts")]
    public Transform HeadPoint;
    public Transform TorsoPoint;
    public Transform LeftHandPoint;
    public Transform RightHandPoint;

    // Visual Body Parts
    private Item Helmet;
    private Item Armor;
    private Item LeftArm;
    private Item RightArm;

    public void DisplayItem(ItemSlot slot, Item instantiatedItem) {
        switch (slot) {
            case ItemSlot.Head:
                instantiatedItem.transform.SetParent(HeadPoint);
                Helmet = instantiatedItem;
                break;
            case ItemSlot.Torso:
                instantiatedItem.transform.SetParent(TorsoPoint);
                Armor = instantiatedItem;
                break;
            case ItemSlot.LeftArm:
                instantiatedItem.transform.SetParent(LeftHandPoint);
                LeftArm = instantiatedItem;
                break;
            case ItemSlot.RightArm:
                instantiatedItem.transform.SetParent(RightHandPoint);
                RightArm = instantiatedItem;
                break;
        }

        instantiatedItem.gameObject.name = instantiatedItem.Name;
        instantiatedItem.transform.localScale = Vector3.one;
        instantiatedItem.transform.localPosition = Vector3.zero;
    }

    public void ClearSlot(ItemSlot slot) {
        switch (slot) {
            case ItemSlot.Head:
                Destroy(Helmet.gameObject);
                Helmet = null;
                break;
            case ItemSlot.Torso:
                Destroy(Armor.gameObject);
                Armor = null;
                break;
            case ItemSlot.LeftArm:
                Destroy(LeftArm.gameObject);
                LeftArm = null;
                break;
            case ItemSlot.RightArm:
                Destroy(RightArm.gameObject);
                RightArm = null;
                break;
        }
    }

}