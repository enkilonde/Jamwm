using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour {

    private static readonly List<Vector3> SpawnPositionOffsets = new List<Vector3>() {
        new Vector3(0,0,3),
        new Vector3(2.8f,0,0.8f),
        new Vector3(1.1f,0,-2.1f),
        new Vector3(-1.1f,0,-2.1f),
        new Vector3(-2.8f,0,0.8f)
    };

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

    public void SpawnLoots(List<Item> loots, Vector3 worldPosition) {
        for (int i = 0; i < loots.Count; i++) {
            SpawnLoot(loots[i], worldPosition + SpawnPositionOffsets[i]);
        }
    }

    public Item SpawnLoot(Item specificItem, Vector3 worldPosition) {
        // TODO : spawn with animation (scale Up from nothing ? Falling from player's height ?)
        Item lootModel = specificItem ?? ItemDatabase.GetRandomItem();
        Item lootObject = Instantiate(
            original: lootModel, 
            parent: RoomManager.Instance._currentRoom.transform,
            position: worldPosition, 
            rotation: Quaternion.identity
        );

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