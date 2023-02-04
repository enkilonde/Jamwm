using UnityEngine;

public class PlayerVisual : MonoBehaviour {

    public ParticleSystem dashFX;

    [Header("Body Parts")]
    public Transform HeadPoint;
    public Transform TorsoPoint;
    public Transform LeftHandPoint;
    public Transform RightHandPoint;

    // Visual Body Parts
    private GameObject Helmet;
    private GameObject Armor;
    private GameObject LeftArm;
    private GameObject RightArm;

    public void DisplayItem(ItemSlot slot, Item item) {
        switch (slot) {
            case ItemSlot.Head:
                Helmet = Instantiate(original: item.EquippedPrefab, parent: HeadPoint);
                break;
            case ItemSlot.Torso:
                Armor = Instantiate(original: item.EquippedPrefab, parent: TorsoPoint);
                break;
            case ItemSlot.LeftArm:
                LeftArm = Instantiate(original: item.EquippedPrefab, parent: LeftHandPoint);
                break;
            case ItemSlot.RightArm:
                RightArm = Instantiate(original: item.EquippedPrefab, parent: RightHandPoint);
                break;
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