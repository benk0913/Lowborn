using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StructureData
{
    [SerializeField]
    public string Name;

    [SerializeField]
    public string Description;

    [SerializeField]
    ValueData PriceValue;

    [SerializeField]
    public int SizeX = 1;

    [SerializeField]
    public int SizeZ = 0;

    [SerializeField]
    public StructureType Type;


    public enum StructureType
    {
        Wall,
        Floor,
        Decoration
    }
}
