using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolveItemUI : MonoBehaviour
{
    [SerializeField]
    Image itemPanel;

    [SerializeField]
    GameObject ItemResolvePanel;

    Item CurrentItemToResolve;
    int CurrentAmount;

    public void Show(Item item, int amount)
    {
        if (this.gameObject.activeInHierarchy)
        {
            return;
        }

        this.gameObject.SetActive(true);
        PlayTool.Instance.PauseTime();

        itemPanel.sprite = item.Icon;
        ItemResolvePanel.gameObject.SetActive(true);

        CurrentItemToResolve = item;
        CurrentAmount = amount;
    }

    public void Show()
    {
        if(this.gameObject.activeInHierarchy)
        {
            return;
        }

        this.gameObject.SetActive(true);
        PlayTool.Instance.PauseTime();

        CurrentItemToResolve = null;

        ItemResolvePanel.gameObject.SetActive(false);
    }

    public void Resolve()
    {
        if(CurrentItemToResolve != null)
        {
            if (CORE.Instance.CurrentScenario.PlayerDynasty.Storage.AddItem(CurrentItemToResolve, CurrentAmount))
            {
                CurrentItemToResolve = null;
            }
        }

        this.gameObject.SetActive(false);
        PlayTool.Instance.ResumeTime();
    }

    public void Cancel()
    {
        this.gameObject.SetActive(false);
        PlayTool.Instance.ResumeTime();

        
    }



}
