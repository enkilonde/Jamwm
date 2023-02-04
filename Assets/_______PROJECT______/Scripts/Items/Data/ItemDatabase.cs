using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Jam/ItemDatabase", fileName = "ItemDatabase", order = 0)]
public class ItemDatabase : SerializedScriptableObject {

    [SerializeField] private Dictionary<ItemID, Item> Items;

    public Item GetRandomItem() {
        return Items[(ItemID)UnityEngine.Random.Range(0, Items.Count)];
    }
    
    public Item GetItem(ItemID itemID) {
        return Items[itemID];
    }

}