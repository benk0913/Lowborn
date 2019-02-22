using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "ItemGroup", menuName = "DataObjects/ItemGroup", order = 2)]
public class ItemGroup : ScriptableObject
{
    public List<Item> Items = new List<Item>();

    public bool ContainsItem(Item item)
    {
        for(int i=0;i<Items.Count;i++)
        {
            if(Items[i].name == item.name)
            {
                return true;
            }
        }

        return false;
    }
}
