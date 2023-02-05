using UnityEngine;

public class PlayerVisual : MonoBehaviour {

    public ParticleSystem dashFX;
    public ParticleSystem hitFx;

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
    private Item Ring; // not really visible, but kept the same way as others for simplicity

    public void DisplayItem(ItemSlot slot, Item instantiatedItem) {
        instantiatedItem.transform.SetParent(null);
        instantiatedItem.transform.localScale = Vector3.one;


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
            case ItemSlot.Ring1:
                Ring = instantiatedItem;
                break;
        }

        instantiatedItem.gameObject.name = instantiatedItem.Name;
        instantiatedItem.transform.localPosition = Vector3.zero;
        instantiatedItem.transform.localRotation = Quaternion.identity;

        if (slot == ItemSlot.Ring1) {
            Ring.transform.position = Vector3.one * 10000; // b-bye
        }
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
            case ItemSlot.Ring1:
                Destroy(Ring.gameObject);
                Ring = null;
                break;
        }
    }

}