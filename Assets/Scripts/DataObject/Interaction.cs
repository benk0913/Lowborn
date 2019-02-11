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
    public string InteractionAnimationTrigger;

    [SerializeField]
    public float Duration;

    public UnityEvent OnComplete;



}
