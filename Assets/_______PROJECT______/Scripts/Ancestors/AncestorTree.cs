using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "AncestorTree", menuName = "ScriptableObjects/AncestorTree", order = 1)]
public class AncestorTree : ScriptableObject
{
    public AncestorData You;

    [Button]
    public void BuildTree(int depth)
    {
        Debug.Log("build tree");
        if (You.Parents == null)
            You.Parents = new List<AncestorData>();
        else
            You.Parents.Clear();

        CreateParents(You, depth);

    }

    private void CreateParents(AncestorData ancestor, int depth)
    {
        Debug.Log("create parent"+depth);
        ancestor.Parents.Add(new AncestorData("Parent A depth"+depth, depth));
        ancestor.Parents.Add(new AncestorData("Parent B depth"+depth, depth));
        depth--;

        if (depth>0)
        {
            foreach (var parent in ancestor.Parents)
            {
                CreateParents(parent, depth);
            }
        }
    }
}
