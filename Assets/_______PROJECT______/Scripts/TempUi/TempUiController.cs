using UnityEngine;
using UnityEngine.UI;

public class TempUiController : MonoBehaviour {

    public static TempUiController Instance;

    [SerializeField] private Text _roomId;
    [SerializeField] private Text _ancestorName;

    private void Awake() {
        Instance = this;
    }

    public void UpdateRoomLevel(int roomLevel) {
        _roomId.text = "Room " + roomLevel;
    }

    public void UpdateAncestorName(string name) {
        _ancestorName.text = name;
    }

}
