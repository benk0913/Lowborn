using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameEventLoseStorageWeight", menuName = "DataObjects/GameEvents/GameEventLoseStorageWeight", order = 2)]
public class GameEventLoseStorageWeight : GameEvent
{
    public override void ExecuteEvent(Object exectuer = null, Object target = null)
    {
        base.ExecuteEvent(exectuer, target);

        if (exectuer == null)
        {
            Debug.LogError("NEED EXECUTING PROP TO ADD IT'S WEIGHT.");
            return;
        }

        ((StructureProp)exectuer).Owner.Storage.WeightCap -= ((PropContainer)((StructureProp)exectuer).Data).ContainerStorageSize;
    }
}
