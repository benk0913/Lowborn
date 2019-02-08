using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModeUI : MonoBehaviour
{
    [SerializeField]
    PortraitUI Portrait;

    private void Awake()
    {
        Portrait.SetCharacter(CORE.Instance.CurrentScenario.PlayerDynasty.HeadOfHouse);
    }
}
