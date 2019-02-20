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

    public void Show(Item item)
    {
        this.gameObject.SetActive(true);
        PlayTool.Instance.PauseTime();

        itemPanel.sprite = item.Icon;
        ItemResolvePanel.gameObject.SetActive(true);

        CurrentItemToResolve = item;
    }

    public void Show()
    {
        this.gameObject.SetActive(true);
        PlayTool.Instance.PauseTime();

        CurrentItemToResolve = null;

        ItemResolvePanel.gameObject.SetActive(false);
    }

    public void Resolve()
    {
        if(CurrentItemToResolve != null)
        {
            CORE.Instance.CurrentScenario.PlayerDynasty.Storage.AddItem(CurrentItemToResolve);
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
