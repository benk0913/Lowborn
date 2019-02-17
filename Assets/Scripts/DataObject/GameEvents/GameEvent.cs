using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "DataObjects/GameEvents/GameEvent", order = 2)]
public class GameEvent : ScriptableObject
{
    public virtual void ExecuteEvent(Actor exectuer = null, InteractableEntity entity = null)
    { 
    }
}
