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

    public Item DisplayItem(ItemSlot slot, Item item) {
        switch (slot) {
            case ItemSlot.Head:
                Helmet = Instantiate(original: item, parent: HeadPoint);
                return Helmet;
            case ItemSlot.Torso:
                Armor = Instantiate(original: item, parent: TorsoPoint);
                return Armor;
            case ItemSlot.LeftArm:
                LeftArm = Instantiate(original: item, parent: LeftHandPoint);
                return LeftArm;
            case ItemSlot.RightArm:
                RightArm = Instantiate(original: item, parent: RightHandPoint);
                return RightArm;
                default: return null;
        }
    }

    public void ClearSlot(ItemSlot slot) {
        switch (slot) {
            case ItemSlot.Head:
                Destroy(Helmet);
                Helmet = null;
                break;
            case ItemSlot.Torso:
                Destroy(Armor);
                Armor = null;
                break;
            case ItemSlot.LeftArm:
                Destroy(LeftArm);
                LeftArm = null;
                break;
            case ItemSlot.RightArm:
                Destroy(RightArm);
                RightArm = null;
                break;
        }
    }

}