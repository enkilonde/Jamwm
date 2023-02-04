using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour {

    public static SaveManager Instance;

    public readonly List<AncestorData> DefeatedAncestors = new List<AncestorData>();

    private void Awake() {
        Instance = this;
    }

    public void HandleDefeatedAncestor(AncestorData vanquished) {
        DefeatedAncestors.Add(vanquished);
    }

    public void ClearCurrentData() {
        DefeatedAncestors.Clear();
    }

}