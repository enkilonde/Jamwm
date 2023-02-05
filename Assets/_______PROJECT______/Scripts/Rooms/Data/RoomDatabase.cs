using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Jam/RoomDatabase", fileName = "RoomDatabase", order = 0)]
public class RoomDatabase : ScriptableObject {

    [SerializeField] private List<BossRoomController> PossibleRooms;

    public BossRoomController GetRandomRoomModel() {
        int r = Random.Range(0, PossibleRooms.Count);
        return PossibleRooms[r];
    }

}