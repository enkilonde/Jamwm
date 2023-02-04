using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LootSpawner : MonoBehaviour {

    public static LootSpawner Instance;

    [Header("Resources")]
    public LootableItem LootPrefab;
    public ItemDatabase ItemDatabase;

    private void Awake() {
        Instance = this;
    }

    public void SpawnLoot(Item specificItem, Vector3 worldPosition) {
        worldPosition += new Vector3(
            Random.Range(0.5f, 1.5f),
            0,
            Random.Range(0.5f, 1.5f)
        ).normalized;

        // TODO : spawn with animation (scale Up from nothing ? Falling from player's height ?)
        var lootObject = Instantiate(original: LootPrefab, position: worldPosition, rotation: Quaternion.identity);

        Item lootItem = specificItem ?? ItemDatabase.GetRandomItem();

        if (specificItem == null) {
            // TODO : apply some stats modifiers based on current progression / level
        }

        lootObject.Configure(lootItem);

        Debug.Log("Spawned loot " + lootItem.Name);
    }

}