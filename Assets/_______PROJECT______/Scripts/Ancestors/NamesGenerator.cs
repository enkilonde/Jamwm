using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NamesData", menuName = "Scriptable Objects/Names Data", order = 2)]
public class NamesGenerator : ScriptableObject
{
    public List<string> Names;
    public List<string> Surnames;
    
    [Button]
    public void AddToNames(string names)
    {
        Names = new List<string>();

        var splitNames =names.Split(' ').ToList();
        Names.AddRange(splitNames);
    }
    [Button]
    public void AddToSurNames(string surnames)
    {
        Surnames = new List<string>();

        var splitNames =surnames.Split(' ').ToList();
        Surnames.AddRange(splitNames);
    }

    public string GenerateName()
    {
        return Names[Random.Range(0, Names.Count - 1)]+" " + Surnames[Random.Range(0, Surnames.Count - 1)];
    }
}
