using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "VC Set", menuName = "DataObjects/VC Set", order = 2)]
public class VCSet : ScriptableObject
{
    [SerializeField]
    public Character.GenderType Gender;

    [SerializeField]
    public List<VisualCharacteristic> SkinColor = new List<VisualCharacteristic>();

    [SerializeField]
    public List<VisualCharacteristic> HairColor = new List<VisualCharacteristic>();

    [SerializeField]
    public List<VisualCharacteristic> Clothing = new List<VisualCharacteristic>();
}
