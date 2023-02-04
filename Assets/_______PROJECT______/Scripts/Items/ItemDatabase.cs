using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jam/ItemDatabase", fileName = "ItemDatabase", order = 0)]
public class ItemDatabase : ScriptableObject {

    public List<Item> Items;

}