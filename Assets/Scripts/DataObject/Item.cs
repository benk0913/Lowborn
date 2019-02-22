using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item", menuName = "DataObjects/Item", order = 2)]
public class Item : ScriptableObject
{
    [SerializeField]
    public Sprite Icon;

    [SerializeField][TextArea(2,4)]
    public string Description;

    [SerializeField]
    public int Value;

    [SerializeField]
    public int Weight;
    
}
