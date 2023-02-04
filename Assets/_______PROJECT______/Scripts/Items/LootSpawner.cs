using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LootSpawner : MonoBehaviour {

    public static LootSpawner Instance
    {
        get
        {
            if(_Instance == null)
                _Instance = FindObjectOfType<LootSpawner>();
            return _Instance;
        }
    }
    private static LootSpawner _Instance;
    

    [Header("Resources")]
    public ItemDatabase ItemDatabase;

    public Item SpawnLoot(Item specificItem, Vector3 worldPosition) {
        worldPosition += new Vector3(
            Random.Range(0.5f, 1.5f),
            0,
            Random.Range(0.5f, 1.5f)
        ).normalized;

        // TODO : spawn with animation (scale Up from nothing ? Falling from player's height ?)
        Item lootModel = specificItem ?? ItemDatabase.GetRandomItem();
        Item lootObject = Instantiate(original: lootModel, position: worldPosition, rotation: Quaternion.identity);

        if (specificItem == null) {
            // TODO : apply some stats modifiers based on current progression / level
        }

        Debug.Log("Spawned loot " + lootModel.Name);
        return lootModel;
    }
/*
    private void Start() {
        SpawnLoot(null, new Vector3(0, 0.25f, 5));
    }
*/
}