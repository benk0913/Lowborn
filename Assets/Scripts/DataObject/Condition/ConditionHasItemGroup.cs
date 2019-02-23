using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ConditionHasItemGroup", menuName = "DataObjects/Conditions/ConditionHasItemGroup", order = 2)]
public class ConditionHasItemGroup : Condition
{
    [SerializeField]
    public ItemGroup ItemGroup;

    [SerializeField]
    public int Amount = 1;

    [SerializeField]
    Equation ConditionEquation = Equation.Equals;

    public override bool SpecificCondition(Object executer = null, Object target = null)
    {
        foreach(string itemKey in CORE.Instance.CurrentScenario.PlayerDynasty.Storage.Items.Keys)
        {
            if(ItemGroup.ContainsItem(CORE.Instance.CurrentScenario.PlayerDynasty.Storage.Items[itemKey].ItemIdentity))
            {
                if(Util.EquationResult(
                CORE.Instance.CurrentScenario.PlayerDynasty.Storage.Items[itemKey].Amount,
                ConditionEquation,
                Amount))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
