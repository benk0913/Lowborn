using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Inventory
{
    public int WeightCap
    {
        get
        {
            return weightCap;
        }
        set
        {
            weightCap = value;

            if(weightCap < TotalWeight)
            {
                InventoryOutOfStorage.Invoke();
            }
        }
    }
    int weightCap;

    public int TotalWeight
    {
        get
        {
            int TotalAmount = 0;
            for(int i=0;i<Items.Keys.Count;i++)
            {
                TotalAmount += Items[Items.Keys.ElementAt(i)].TotalWeight;
            }

            return TotalAmount;
        }
    }

    public Dictionary<string, InventoryItem> Items { private set; get; }

    public UnityEvent InventoryChanged = new UnityEvent();
    public UnityEvent InventoryOutOfStorage = new UnityEvent();
    public CannotAddItemEvent ItemFailedEvent = new CannotAddItemEvent();


    public Inventory()
    {
        Items = new Dictionary<string, InventoryItem>();
        WeightCap = 5;
    }


    public bool AddItem(Item item, int amount = 1)
    {
        if(TotalWeight + ( item.Weight * amount ) > WeightCap)
        {
            ItemFailedEvent.Invoke(item, amount);
            return false;
        }

        if(Items.ContainsKey(item.name))
        {
            Items[item.name].Amount += amount;

            InventoryChanged.Invoke();
            return true;
        }

        Items.Add(item.name, new InventoryItem(item, amount));

        InventoryChanged.Invoke();
        return true;
    }

    public bool RemoveItem(Item item, int amount = 1)
    {
        if (!Items.ContainsKey(item.name))
        {
            return false;
        }

        if(Items[item.name].Amount < amount)
        {
            return false;
        }



        Items[item.name].Amount -= amount;

        if (Items[item.name].Amount <= 0)
        {
            Items.Remove(item.name);
        }

        InventoryChanged.Invoke();


        if(TotalWeight > WeightCap)
        {
            InventoryOutOfStorage.Invoke();
        }

        return true;
    }

    public Item RemoveItemOfGroup(ItemGroup Group)
    {
        for(int i=0;i<Items.Keys.Count;i++)
        {
            if(Group.ContainsItem(Items[Items.Keys.ElementAt(i)].ItemIdentity))
            {
                Item itemRemoved = Items[Items.Keys.ElementAt(i)].ItemIdentity;
                RemoveItem(Items[Items.Keys.ElementAt(i)].ItemIdentity);
                return itemRemoved;
            }
        }

        return null;
    }

    public class InventoryItem
    {
        public Item ItemIdentity;
        public int Amount = 1;

        public int TotalWeight
        {
            get
            {
                return Amount * ItemIdentity.Weight;
            }
        }

        public int TotalValue
        {
            get
            {
                return Amount * ItemIdentity.Value;
            }
        }

        public InventoryItem(Item item, int amount)
        {
            this.ItemIdentity = item;
            this.Amount = amount;
        }
    }

    public class CannotAddItemEvent : UnityEvent<Item, int>
    {

    }
}
