using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    Transform ItemContainer;

    [SerializeField]
    TextMeshProUGUI TotalWeight; 

    Inventory CurrentInventory;

    public void SetInfo(Inventory inventory)
    {
        if(CurrentInventory != null)
        {
            CurrentInventory.InventoryChanged.RemoveListener(OnInventoryChanged);
            CurrentInventory = null;
        }

        CurrentInventory = inventory;
        CurrentInventory.InventoryChanged.AddListener(OnInventoryChanged);

        OnInventoryChanged();
    }

    private void OnEnable()
    {
        if(CurrentInventory != null)
        {
            CurrentInventory.InventoryChanged.AddListener(OnInventoryChanged);
            OnInventoryChanged();
        }

    }

    private void OnDisable()
    {
        if (CurrentInventory != null)
        {
            CurrentInventory.InventoryChanged.RemoveListener(OnInventoryChanged);
        }
    }

    public void OnInventoryChanged()
    {
        Clear();

        InventoryItemUI tempItem;
        for(int i=0;i<CurrentInventory.Items.Keys.Count;i++)
        {
            tempItem = ResourcesLoader.Instance.GetRecycledObject("InventoryItemUI").GetComponent<InventoryItemUI>();
            tempItem.transform.SetParent(ItemContainer, false);
            tempItem.SetInfo(CurrentInventory.Items[CurrentInventory.Items.Keys.ElementAt(i)]);
        }

        TotalWeight.text = CurrentInventory.TotalWeight + " / " + CurrentInventory.WeightCap;
    }

    public void Clear()
    {
        while (ItemContainer.childCount > 0)
        {
            ItemContainer.GetChild(0).gameObject.SetActive(false);
            ItemContainer.GetChild(0).transform.SetParent(transform);
        }
    }
}
