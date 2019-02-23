using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ConditionHasItem", menuName = "DataObjects/Conditions/ConditionHasItem", order = 2)]
public class ConditionHasItem : Condition
{
    [SerializeField]
    public Item ItemToCheck;

    [SerializeField]
    public int Amount = 1;
    
    [SerializeField]
    Equation ConditionEquation = Equation.Equals;

    public override bool SpecificCondition(Object executer = null, Object target = null)
    {
        if(!CORE.Instance.CurrentScenario.PlayerDynasty.Storage.Items.ContainsKey(ItemToCheck.name))
        {
            return Util.EquationResult(
            0,
            ConditionEquation,
            Amount);
        }

        return Util.EquationResult(
           CORE.Instance.CurrentScenario.PlayerDynasty.Storage.Items[ItemToCheck.name].Amount,
           ConditionEquation,
           Amount);
    }
}
