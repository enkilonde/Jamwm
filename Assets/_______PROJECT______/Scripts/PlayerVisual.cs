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

    public Item DisplayItem(ItemSlot slot, Item model) {
        Item spawned = null;
        switch (slot) {
            case ItemSlot.Head:
                spawned = Instantiate(original: model, parent: HeadPoint);
                break;
            case ItemSlot.Torso:
                spawned = Instantiate(original: model, parent: TorsoPoint);
                break;
            case ItemSlot.LeftArm:
                spawned = Instantiate(original: model, parent: LeftHandPoint);
                break;
            case ItemSlot.RightArm:
                spawned = Instantiate(original: model, parent: RightHandPoint);
                break;
        }
        if (spawned != null) {
            spawned.gameObject.name = spawned.Name;
            spawned.transform.localScale = Vector3.one;
            spawned.transform.localPosition = Vector3.zero;
        }
        return spawned;
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