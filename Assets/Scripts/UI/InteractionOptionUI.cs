using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InteractionOptionUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI Text;

    Interaction CurrentInteraction;
    InteractableEntity CurrentEntity;

    public void SetInfo(InteractableEntity entity, Interaction interaction)
    {
        this.CurrentEntity = entity;
        this.CurrentInteraction = interaction;
        Text.text = CurrentInteraction.name;
    }

    public void SelectOption()
    {
        PlayTool.Instance.SelectInteraction(CurrentEntity, CurrentInteraction);
    }
}
