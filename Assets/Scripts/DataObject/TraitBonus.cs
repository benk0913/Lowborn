using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TraitBonus", menuName = "DataObjects/TraitBonus", order = 2)]
public class TraitBonus : ScriptableObject
{
    public BonusType Type;
    public string Key;
    public string Value;
}

[System.Serializable]
public enum BonusType
{
    Relations,
    Need,
}
