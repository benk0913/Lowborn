using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEventGainRandomItemOfGroup", menuName = "DataObjects/GameEvents/GameEventGainRandomItemOfGroup", order = 2)]
public class GameEventGainRandomItemOfGroup : GameEvent
{
    public ItemGroup Group;
    public int Amount;

    public bool ShowHover = true;

    public override void ExecuteEvent(Object exectuer = null, Object entity = null)
    {
        base.ExecuteEvent(exectuer, entity);

        Item itemOfRelevance = null;

        if (Amount > 0) // Positive Amount 
        {
            itemOfRelevance = Group.Items[Random.Range(0, Group.Items.Count)];
            CORE.Instance.CurrentScenario.PlayerDynasty.Storage.AddItem(itemOfRelevance, Amount);

            if (!ShowHover)
            {
                return;
            }

            if (exectuer == null)
            {
                return;
            }

            HoveringGainUI HoverComponent = ResourcesLoader.Instance.GetRecycledObject("HoveringGain").GetComponent<HoveringGainUI>();
            HoverComponent.transform.SetParent(PlayModeUI.Instance.transform);

            HoverComponent.transform.position =
                Camera.main.WorldToScreenPoint(((Actor)exectuer).transform.position) + new Vector3(Random.Range(-25f, 25f), Random.Range(-25f, 25f), 0f);

            HoverComponent.SetInfo(itemOfRelevance.Icon, Amount);
        }
        else // Negative Amount
        {
            Item previousItem = null;
            int internalAmount = 0;

            for (int i = 0; i < Mathf.Abs(Amount); i++)
            {
                if(itemOfRelevance != null)
                {
                    previousItem = itemOfRelevance;
                }

                itemOfRelevance = CORE.Instance.CurrentScenario.PlayerDynasty.Storage.RemoveItemOfGroup(Group);

                internalAmount++;

                if (itemOfRelevance != previousItem && previousItem != null)
                {
                    HoveringGainUI HoverComponent = ResourcesLoader.Instance.GetRecycledObject("HoveringGain").GetComponent<HoveringGainUI>();
                    HoverComponent.transform.SetParent(PlayModeUI.Instance.transform);

                    HoverComponent.transform.position =
                        Camera.main.WorldToScreenPoint(((Actor)exectuer).transform.position) + new Vector3(Random.Range(-25f, 25f), Random.Range(-25f, 25f), 0f);

                    HoverComponent.SetInfo(previousItem.Icon, -1 * internalAmount);

                    internalAmount = 0;
                }
            }
        }



    }
}
