using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "GameEvent", menuName = "DataObjects/GameEvents/GameEventGainNeed", order = 2)]
public class GameEventGainNeed : GameEvent
{
    public Need needType;
    public float MinPrecent;
    public float MaxPrecent;

    public bool ShowHover = true;

    public override void ExecuteEvent(Object exectuer = null, Object entity = null)
    {
        base.ExecuteEvent(exectuer, entity);

        float RandomPrecent = Random.Range(MinPrecent, MaxPrecent);
        CORE.Instance.CurrentScenario.PlayerDynasty.HeadOfHouse.GetNeed(needType).Raise(RandomPrecent);

        if(!ShowHover)
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

        HoverComponent.SetInfo(needType.Icon, RandomPrecent);
    }
}
