using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    Session SelectedScenario;

    public void SelectScenario(Session scenario)
    {
        this.SelectedScenario = scenario;
    }
}
