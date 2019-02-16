using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dynasty", menuName = "DataObjects/Dynasty", order = 2)]
public class Dynasty : ScriptableObject
{
    public string Name;
    public string ID;
    public Character HeadOfHouse;

    public Inventory Storage = new Inventory();

    public Dynasty()
    {
        this.ID = Util.GenerateUniqueID();
    }
}
