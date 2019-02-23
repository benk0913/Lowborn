using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Condition", menuName = "DataObjects/Conditions/Condition", order = 2)]
public class Condition : ScriptableObject
{
    [SerializeField]
    public bool DefaultState = false;

    [SerializeField]
    public bool Opposite = false;

    public bool Validate()
    {
        return Opposite ? !SpecificCondition() : SpecificCondition();
    }


    /// <summary>
    /// Extend this function (not validate)
    /// </summary>
    public virtual bool SpecificCondition(Object executer = null, Object target = null)
    {
        return DefaultState;
    }
}
