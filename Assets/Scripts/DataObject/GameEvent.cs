using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameEvent", menuName = "DataObjects/GameEvent", order = 2)]
public class GameEvent : ScriptableObject
{
    [SerializeField]
    public GameEventType Type;

    public List<ScriptableObject> Objects = new List<ScriptableObject>();

    public void ExecuteEvent(Actor exectuer = null, InteractableEntity entity = null)
    {
        switch(Type)
        {
            case GameEventType.GainItem :
                {
                    for(int i=0;i<Objects.Count;i++)
                    {
                        CORE.Instance.CurrentScenario.PlayerDynasty.Storage.AddItem((Item)Objects[i]);
                    }
                    return;
                }
        }
    }

    public enum GameEventType
    {
        GainItem
    }
}
