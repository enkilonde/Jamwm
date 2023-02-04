using System;
using UnityEngine;

public class RoomManager : MonoBehaviour {

    public static RoomManager Instance;

    [Header("Resources")]
    [SerializeField] private ChoiceRoomController _choiceRoomPrefab;
    [SerializeField] private BossRoomController _bossRoomPrefab;

    [Header("Loaded Rooms")]
    private ChoiceRoomController _currentChoiceRoom;
    private BossRoomController _leftBossRoom;
    private BossRoomController _rightBossRoom;
    
    public RoomType CurrentRoomType;

    // TODO : might have to be kept elsewhere
    public int CurrentLevel;

    private void Awake() {
        Instance = this;
        CurrentRoomType = RoomType.Choice;
    }

    public void HandleBossDeath() {
        // TODO : generate next "choice" room
    }

    public void HandleBossRoomLeft() {
        // TODO : generate the 2 next boss room
    }

    public void UnloadRoom(RoomType roomType) {
        switch (roomType) {
            case RoomType.Choice:
                Destroy(_currentChoiceRoom);
                _currentChoiceRoom = null;
                return;
            case RoomType.Combat:
                Destroy(_leftBossRoom);
                Destroy(_rightBossRoom);
                _leftBossRoom = null;
                _rightBossRoom = null;
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(roomType), roomType, null);
        }
    }

}