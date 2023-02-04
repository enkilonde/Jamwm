using UnityEngine;

public class LootSpawner : MonoBehaviour {

    [Header("Resources")]
    public LootableItem LootPrefab;
    public ItemDatabase ItemDatabase;

    public void SpawnLoot(ItemID? specificItem, Vector3 worldPosition) {
        Instantiate(original: LootPrefab, position: worldPosition, rotation: Quaternion.identity);

        Item lootItem;
        if (specificItem.HasValue) {
            lootItem = ItemDatabase.GetItem(specificItem.Value);
        } else {
            lootItem = ItemDatabase.GetRandomItem();
        }

        // TODO : apply some stats modifiers based on current progression / level

        LootPrefab.Loot = lootItem;
    }

}