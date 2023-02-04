using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class UIAncestorTree : MonoBehaviour
{
    [SerializeField]private AncestorTree _ancestorTree;
    
    [SerializeField, BoxGroup("Links")]private GameObject _ancestorPefab;
    [SerializeField, BoxGroup("Links")]private GameObject _layoutGroupPrefab;

    private Dictionary<int, HorizontalLayoutGroup> _layoutGroups;
    [Button]
    public void DisplayTree()
    {
        ClearChildren();
        DisplayAncestor(_ancestorTree.You);
    }

    [Button]
    private void ClearChildren()
    {
        _layoutGroups?.Clear();
        foreach (Transform child in transform) {
            DestroyImmediate(child.gameObject);
        }
    }

    [Button]
    private void DisplayAncestor(AncestorData ancestor)
    {
        if (_layoutGroups == null)
            _layoutGroups = new Dictionary<int, HorizontalLayoutGroup>();

        
        if (!_layoutGroups.ContainsKey(ancestor.Depth))
        {
            _layoutGroups.Add(ancestor.Depth,Instantiate(_layoutGroupPrefab, transform).GetComponent<HorizontalLayoutGroup>() );
        }

        var ancestorUI =Instantiate(_ancestorPefab, _layoutGroups[ancestor.Depth].transform).GetComponent<UIAncestor>();
        ancestorUI.Setup(ancestor.Name, null, ancestor.Depth);

        foreach (var parent in ancestor.Parents)
        {
            DisplayAncestor(parent);
        }
    }
}
