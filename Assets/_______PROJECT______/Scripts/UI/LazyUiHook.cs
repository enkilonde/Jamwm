using UnityEngine;

public class LazyUiHook : MonoBehaviour {

    public static LazyUiHook Instance;

    [SerializeField] public LifeBar PlayerLifeBar;
    [SerializeField] public LifeBar BossLifeBar;

    private void Awake() {
        Instance = this;
    }

}