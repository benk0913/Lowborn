using System;
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

    [SerializeField]
    ResolveItemUI ResolveStoragePanel;

    [SerializeField]
    InventoryItemUI CurrentlySelectedItem;

    [SerializeField]
    GameObject SelectedItemControlPanel;

    [SerializeField]
    TextMeshProUGUI SelectedItemNameText;

    [SerializeField]
    TextMeshProUGUI SelectedItemAmountText;

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
            CurrentInventory.InventoryOutOfStorage.AddListener(OnInventoryOutOfStorage);
            
            OnInventoryChanged();
        }

    }

    private void OnDisable()
    {
        if (CurrentInventory != null)
        {
            CurrentInventory.InventoryChanged.RemoveListener(OnInventoryChanged);
            CurrentInventory.InventoryOutOfStorage.RemoveListener(OnInventoryOutOfStorage);
        }
    }

    internal void OnItemFailed(Item item)
    {
        this.gameObject.SetActive(true);
        ResolveStoragePanel.Show(item);

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

        if(CurrentInventory.TotalWeight <= CurrentInventory.WeightCap)
        {
            ResolveStoragePanel.Resolve();
        }
    }

    public void OnInventoryOutOfStorage()
    {
        this.gameObject.SetActive(true);
        ResolveStoragePanel.Show();
    }

    public void Clear()
    {
        while (ItemContainer.childCount > 0)
        {
            ItemContainer.GetChild(0).gameObject.SetActive(false);
            ItemContainer.GetChild(0).transform.SetParent(transform);
        }
    }

    public void RemoveSelectedItem()
    {
        CurrentInventory.RemoveItem(CurrentlySelectedItem.CurrentItem.ItemIdentity, 1);

        if(!CurrentInventory.Items.ContainsKey(CurrentlySelectedItem.CurrentItem.ItemIdentity.name))
        {
            Deselect();
            return;
        }

        SelectedItemAmountText.text = CurrentlySelectedItem.CurrentItem.Amount.ToString();
    }

    public void SelectItem(InventoryItemUI item)
    {
        CurrentlySelectedItem = item;
        SelectedItemNameText.text = item.CurrentItem.ItemIdentity.name;
        SelectedItemAmountText.text = item.CurrentItem.Amount.ToString();
        SelectedItemControlPanel.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        CurrentlySelectedItem = null;
        SelectedItemControlPanel.SetActive(false);
    }
}
