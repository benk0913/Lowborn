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


    public UnityEvent OnComplete;

    [SerializeField]
    public List<GameEvent> OnCompleteEvenets = new List<GameEvent>();



}
