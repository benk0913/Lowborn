using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildToolUI : MonoBehaviour
{
    [SerializeField]
    Transform IconsContainer;

    private void Start()
    {
        Initialize();    
    }


    private void OnEnable()
    {
        AddListeners();
    }

    private void OnDisable()
    {
        RemoveListeners();
    }

    #region Event Handling
    void AddListeners()
    {
        CORE.Instance.CurrentScenario.PlayerDynasty.Storage.ItemFailedEvent.AddListener(OnItemFailed);
        CORE.Instance.CurrentScenario.PlayerDynasty.Storage.InventoryOutOfStorage.AddListener(OnOutOfStorage);
    }

    void RemoveListeners()
    {
        CORE.Instance.CurrentScenario.PlayerDynasty.Storage.ItemFailedEvent.RemoveListener(OnItemFailed);
        CORE.Instance.CurrentScenario.PlayerDynasty.Storage.InventoryOutOfStorage.RemoveListener(OnOutOfStorage);
    }

    public void OnItemFailed(Item item, int amount)
    {
        LocationMap.Instance.CurrentGameTool = GameTool.Play;
        PlayModeUI.Instance.OnItemFailed(item, amount);
    }

    public void OnOutOfStorage()
    {
        LocationMap.Instance.CurrentGameTool = GameTool.Play;
        PlayModeUI.Instance.OnOutOfStorage();
    }
    #endregion

    private void Initialize()
    {
        GameObject tempItem;
        for(int i=0;i< CORE.Instance.Database.Buildable.Count;i++)
        {
            tempItem = ResourcesLoader.Instance.GetRecycledObject("buildingItem");

            tempItem.transform.SetParent(IconsContainer, false);

            tempItem.GetComponent<SelectableBuildItemUI>().SetInfo(CORE.Instance.Database.Buildable[i]);
        }
    }


}
