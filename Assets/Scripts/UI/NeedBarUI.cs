using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeedBarUI : MonoBehaviour
{
    [SerializeField]
    Image Fill;

    [SerializeField]
    Color GoodColor;

    [SerializeField]
    Color BadColor;

    [SerializeField]
    Need RepresentingNeed;

    Character.NeedBar CurrentNeedBar;

    private void Start()
    {
        List<Character.NeedBar> needs = LocationMap.Instance.Data.PlayerActor.Character.Needs;
        for (int i=0;i< needs.Count;i++)
        {
            if(needs[i].Identity.name == RepresentingNeed.name)
            {
                CurrentNeedBar = needs[i];
                return;
            }
        }

        this.gameObject.SetActive(false);
    }

    public void SetNeed(Character.NeedBar needBar)
    {
        CurrentNeedBar = needBar;
    }

    void Update()
    {
        if(CurrentNeedBar == null)
        {
            return;
        }

        Fill.fillAmount = Mathf.Lerp(Fill.fillAmount, CurrentNeedBar.CurrentPrecent, Time.deltaTime);
        Fill.color = Color.Lerp(BadColor, GoodColor, Fill.fillAmount);
    }
}
