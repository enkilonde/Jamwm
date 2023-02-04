using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AncestorData
{
    public string Name;

    public List<AncestorData> Parents;

    public int Depth;
    public AncestorData(string name, int depth)
    {
        Name = name;
        Depth = depth;
        Parents = new List<AncestorData>();
    }
}
