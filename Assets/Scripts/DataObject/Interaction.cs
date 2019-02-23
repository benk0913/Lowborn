using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "Interaction", menuName = "DataObjects/Interaction", order = 2)]
public class Interaction : ScriptableObject
{
    [SerializeField][TextArea(2,3)]
    public string Description;

    [SerializeField]
    public int InteractionAnimationNumber;

    [SerializeField]
    public float Duration;

    [SerializeField]
    public bool ShowProgressBar = true;

    [SerializeField]
    public bool Repeat = true;

    [SerializeField]
    public bool ReanimateOnRepeat = true;

    [SerializeField]
    public List<GameEvent> OnCompleteEvenets = new List<GameEvent>();

    [SerializeField]
    public List<Condition> RequiredConditions = new List<Condition>();

    [SerializeField]
    public Item InteractionEquip;

    public bool HasRequirements
    {
        get
        {
            for(int i=0;i<RequiredConditions.Count;i++)
            {
                if(!RequiredConditions[i].Validate())
                {
                    return false;
                }
            }

            return true;
        }
    }

}
