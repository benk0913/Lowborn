using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Prop", menuName = "DataObjects/Building/Prop", order = 2)]
public class Prop : ScriptableObject
{
    public PropType Type;

    [TextArea(2,4)]
    public string Description;
    public Sprite Icon;
    public ValueData Price;
    public GameObject Prefab;
    public bool Buildable = true;


    [SerializeField]
    public int SizeX = 1;

    [SerializeField]
    public int SizeZ = 0;

    [SerializeField]
    public PropType _PropType;

    [SerializeField]
    public bool IsBuildable = true;


    [SerializeField]
    public List<GameEvent> OnPlaceEvents  = new List<GameEvent>();

    [SerializeField]
    public List<GameEvent> OnRemoveEvents = new List<GameEvent>();

}
