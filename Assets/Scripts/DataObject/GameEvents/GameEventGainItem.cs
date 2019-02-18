using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "DataObjects/GameEvents/GameEventGainItem", order = 2)]
public class GameEventGainItem : GameEvent
{
    public Item itemType;
    public int MinAmount;
    public int MaxAmount;

    public bool ShowHover = true;

    public override void ExecuteEvent(Object exectuer = null, Object entity = null)
    {
        base.ExecuteEvent(exectuer, entity);

        int RandomAmount = Random.Range(MinAmount, MaxAmount);
        CORE.Instance.CurrentScenario.PlayerDynasty.Storage.AddItem(itemType, RandomAmount);


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

        HoverComponent.SetInfo(itemType.Icon, RandomAmount);
    }
}
