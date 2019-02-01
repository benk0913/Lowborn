using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Visual Characteristic", menuName = "DataObjects/Visual Characteristic", order = 2)]
public class VisualCharacteristic : ScriptableObject
{
    [SerializeField]
    public VCType Type;

    [SerializeField]
    public List<VisualCharacteristic> Pool = new List<VisualCharacteristic>();

    [SerializeField]
    public Sprite SetSprite;

    [SerializeField]
    public Material SetMaterial;

    [SerializeField]
    public Mesh SetMesh;

    public int CurrentIndex = 0;
    
    public void Next()
    {
        CurrentIndex++;
        if(CurrentIndex >= Pool.Count)
        {
            CurrentIndex = 0;
        }
    }

    public void Previous()
    {
        CurrentIndex--;
        if (CurrentIndex < 0)
        {
            CurrentIndex = Pool.Count - 1;
        }
    }

    public VisualCharacteristic GetVCByName(string sName)
    {
        if(this.name == sName)
        {
            return this;
        }

        VisualCharacteristic visualChar = null;
        foreach(VisualCharacteristic vc in Pool)
        {
            visualChar = vc.GetVCByName(sName);

            if (vc != null 
                && vc.name == sName)
            {
                return visualChar;
            }
        }

        return null;
    }
}

public enum VCType
{
    SkinColor,
    Face,
    HairColor,
    Hair,
    Clothing
}
