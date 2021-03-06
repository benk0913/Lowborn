﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI ItemName;

    [SerializeField]
    TextMeshProUGUI ItemWeight;

    [SerializeField]
    TextMeshProUGUI ItemPrice;

    [SerializeField]
    TextMeshProUGUI ItemAmount;

    [SerializeField]
    Image ItemIcon;

    [SerializeField]
    Image ItemBG;

    public Inventory.InventoryItem CurrentItem;

    public void SetInfo(Inventory.InventoryItem item)
    {
        CurrentItem = item;

        ItemIcon.sprite = item.ItemIdentity.Icon;
        ItemName.text = item.ItemIdentity.name;
        ItemWeight.text = "w"+item.TotalWeight;
        ItemPrice.text = "c" + item.TotalValue;
        ItemAmount.text = item.Amount.ToString();
    }

    public void Select()
    {
        PlayModeUI.Instance.StorageInfo.SelectItem(this);
    }

    public void SetSelected()
    {
        ItemBG.color = Color.blue;
    }

    public void Deselect()
    {
        ItemBG.color = Color.white;
    }
}
