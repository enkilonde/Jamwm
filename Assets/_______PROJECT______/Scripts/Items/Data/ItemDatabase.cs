using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jam/ItemDatabase", fileName = "ItemDatabase", order = 0)]
public class ItemDatabase : ScriptableObject {

    public List<Item> Items;

    public Item GetRandomItem() {
        return Items[Random.Range(0, Items.Count)];
    }

    public Item GetRandomItem(ItemKind specificKind) {
        Item chosenItem = null;
        while (chosenItem == null) {
            chosenItem = GetRandomItem();
            if (chosenItem.Kind != specificKind) {
                chosenItem = null;
            }
        }
        return chosenItem;
    }

    public Item GetItem(ItemID itemID) {
        foreach (var item in Items) {
            if (item.ID == itemID) {
                return item;
            }
        }
        Debug.LogError("No item found with id " + itemID);
        return null;
    }

}