using UnityEngine;

public class BossRoomController : MonoBehaviour {

    [Header("Room parts")]
    [SerializeField] private DoorController _leftExitDoor;
    [SerializeField] private DoorController _rightExitDoor;

    [Header("Resources")]
    [SerializeField] private GameObject _enemyPrefab;

    private CustomCharacterController _playerRef;
    private AncestorData _leftDoorChoice;
    private AncestorData _rightDoorChoice;

    public void Configure(AncestorData boss, AncestorData leftChoice, AncestorData rightChoice, 
        CustomCharacterController playerRef) {
        _leftDoorChoice = leftChoice;
        _rightDoorChoice = rightChoice;
        _playerRef = playerRef;

        if (boss.InitialRoomAncestor == false) {
            SpawnAncestor(boss);
        }
    }

    private void SpawnAncestor(AncestorData ancestorData) {
        var ancestor = Instantiate(
            original: _enemyPrefab,
            position: new Vector3(0, 0, 15),
            rotation: Quaternion.identity
        );

        var iaInputs = ancestor.GetComponent<CustomIAInputs>();
        iaInputs.player = _playerRef;
        Debug.LogError("IA Inputs player ref set " + (_playerRef != null));

        var ancestorController = ancestor.GetComponent<CustomCharacterController>();
        var bossSheet = ancestorData.GetDetailedSheet(ancestorController.playerVisual);
        ancestorController.SetBossSheet(bossSheet);
    }

    public void SetExitDoorsLocked(bool locked) {
        _leftExitDoor.SetLocked(locked);
        _rightExitDoor.SetLocked(locked);
    }

    public void HandleDoorPassed(DoorController.DoorKind doorKind) {
        switch (doorKind) {
            case DoorController.DoorKind.LeftExit:
                RoomManager.Instance.TransitionToNextBossRoom(_leftDoorChoice);
                break;
            case DoorController.DoorKind.RightExit:
                RoomManager.Instance.TransitionToNextBossRoom(_rightDoorChoice);
                break;
        }
    }

}