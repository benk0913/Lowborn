using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public static MainMenuUI Instance;

    Session SelectedScenario;

    private void Awake()
    {
        Instance = this;
    }

    public void SelectScenario(Session scenario)
    {
        this.SelectedScenario = scenario;
    }

    public void SelectCharacter(Character character)
    {
        this.SelectedScenario.PlayerDynasty = (Dynasty) ScriptableObject.CreateInstance(typeof(Dynasty));
        this.SelectedScenario.PlayerDynasty.HeadOfHouse = character;
    }

    public void PlaySelected()
    {
        CORE.Instance.PlayScenario(SelectedScenario);
    }
}
