using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    [SerializeField]
    Image SelectedItemImage;

    [SerializeField]
    GameObject CloseButtonObject;

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

    internal void OnItemFailed(Item item, int amount)
    {
        ResolveStoragePanel.Show(item, amount);

        this.gameObject.SetActive(true);
        SetInfo(CORE.Instance.CurrentScenario.PlayerDynasty.Storage);
    }

    public void OnInventoryOutOfStorage()
    {
        ResolveStoragePanel.Show();

        this.gameObject.SetActive(true);
        SetInfo(CORE.Instance.CurrentScenario.PlayerDynasty.Storage);
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

        RefreshCurrentItem();

        TotalWeight.text = CurrentInventory.TotalWeight + " / " + CurrentInventory.WeightCap;

        if (ResolveStoragePanel.gameObject.activeInHierarchy)
        {
            CloseButtonObject.gameObject.SetActive(ResolveStoragePanel.AttemptResolve());
        }
        else
        {
            CloseButtonObject.gameObject.SetActive(true);
        }
    }


    public void Clear()
    {
        while (ItemContainer.childCount > 0)
        {
            ItemContainer.GetChild(0).gameObject.SetActive(false);
            ItemContainer.GetChild(0).transform.SetParent(transform);
        }
    }

    public void RemoveSelectedItem(int Amount = 1)
    {
        CurrentInventory.RemoveItem(CurrentlySelectedItem.CurrentItem.ItemIdentity, Amount);

        if(!CurrentInventory.Items.ContainsKey(CurrentlySelectedItem.CurrentItem.ItemIdentity.name))
        {
            Deselect();
            return;
        }

        SelectedItemAmountText.text = CurrentlySelectedItem.CurrentItem.Amount.ToString();
    }

    public void RemoveSelectedItemAll()
    {
        RemoveSelectedItem(CurrentlySelectedItem.CurrentItem.Amount);
    }

    public void SelectItem(InventoryItemUI item)
    {
        Deselect();
        CurrentlySelectedItem = item;
        RefreshCurrentItem();
        item.SetSelected();
    }

    void RefreshCurrentItem()
    {
        if(CurrentlySelectedItem == null)
        {
            return;
        }

        SelectedItemNameText.text = CurrentlySelectedItem.CurrentItem.ItemIdentity.name;
        SelectedItemAmountText.text = CurrentlySelectedItem.CurrentItem.Amount.ToString();
        SelectedItemImage.sprite = CurrentlySelectedItem.CurrentItem.ItemIdentity.Icon;
        SelectedItemControlPanel.gameObject.SetActive(true);

    }

    public void Deselect()
    {
        if(CurrentlySelectedItem == null)
        {
            return;
        }

        CurrentlySelectedItem.Deselect();
        CurrentlySelectedItem = null;
        SelectedItemControlPanel.SetActive(false);
    }
}
