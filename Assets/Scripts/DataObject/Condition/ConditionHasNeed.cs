using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ConditionHasNeed ", menuName = "DataObjects/Conditions/ConditionHasNeed ", order = 2)]
public class ConditionHasNeed : Condition
{
    [SerializeField]
    public Need NeedToCheck;

    [SerializeField]
    public float Amount = 1f;

    [SerializeField]
    Equation ConditionEquation = Equation.Equals;

    public override bool SpecificCondition(Object executer = null, Object target = null)
    {
        if (CORE.Instance.CurrentScenario.PlayerDynasty.HeadOfHouse.GetNeed(NeedToCheck) == null)
        {
            return Util.EquationResult(
            0,
            ConditionEquation,
            Amount);
        }

        return Util.EquationResult(
            CORE.Instance.CurrentScenario.PlayerDynasty.HeadOfHouse.GetNeed(NeedToCheck).CurrentPrecent,
            ConditionEquation, 
            Amount);
    }
}
