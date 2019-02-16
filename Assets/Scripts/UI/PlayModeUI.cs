using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModeUI : MonoBehaviour
{
    [SerializeField]
    PortraitUI Portrait;

    [SerializeField]
    Transform InteractionOptionsContainer;

    [SerializeField]
    CharacterInfoUI CharacterInfo;

    [SerializeField]
    HouseholdInfoUI HouseholdInfo;

    public static PlayModeUI Instance;

    Coroutine ShowInteractionOptionsRoutineInstance;

    private void Awake()
    {
        Instance = this;

        Portrait.SetCharacter(CORE.Instance.CurrentScenario.PlayerDynasty.HeadOfHouse);
    }

    public void ShowInteractionOptions(InteractableEntity entity)
    {
        if(ShowInteractionOptionsRoutineInstance != null)
        {
            StopCoroutine(ShowInteractionOptionsRoutineInstance);
        }

        ShowInteractionOptionsRoutineInstance = StartCoroutine(ShowInteractionOptionsRoutine(entity));
    }

    IEnumerator ShowInteractionOptionsRoutine(InteractableEntity entity)
    {
        ClearInteractionOptions();

        InteractionOptionsContainer.transform.position = Input.mousePosition;

        InteractionOptionUI tempOption;
        for (int i = 0; i < entity.PossibleInteractions.Count; i++)
        {
            tempOption = ResourcesLoader.Instance.GetRecycledObject("InteractionOptionUI").GetComponent<InteractionOptionUI>();
            tempOption.transform.SetParent(InteractionOptionsContainer, false);

            tempOption.SetInfo(entity, entity.PossibleInteractions[i]._Interaction);

            yield return new WaitForSeconds(0.1f);
        }

        ShowInteractionOptionsRoutineInstance = null;
    }

    public void ClearInteractionOptions()
    {
        if (ShowInteractionOptionsRoutineInstance != null)
        {
            StopCoroutine(ShowInteractionOptionsRoutineInstance);
        }

        while (InteractionOptionsContainer.childCount > 0)
        {
            InteractionOptionsContainer.GetChild(0).gameObject.SetActive(false);
            InteractionOptionsContainer.GetChild(0).SetParent(transform);
        }
    }

    public void ShowCharacterInfo(Character character)
    {
        CharacterInfo.Show(character);
    }

    public void ShowCharacterInfo()
    {
        CharacterInfo.Show(LocationMap.Instance.Data.PlayerActor.Character);
    }

    public void ShowHouseholdInfo()
    {
        HouseholdInfo.Show();
    }
}
