using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private List<ItemSlot> itemSlots;

    public event Action<int, ItemSlot> OnChangedItemSlot = delegate { };

    private const int inventoryCapacity = 50;
    private const int slotCapacity = 50;

    void Awake()
    {
        Initialize();
    }

    public bool AddItem(Item item, int amount)
    {
        if (!CanAdd(item, amount))
            return false;

        if (item is EquipmentItem)
        {
            var result = itemSlots.Where(s => s.IsEmpty).ToList();

            for (int i = 0; i < amount; i++)
            {
                result[i].Add(item, 1);

                OnChangedItemSlot?.Invoke(itemSlots.IndexOf(result[i]), result[i]);
            }

            return true;
        }
        else
        {
            int remaining = amount;

            foreach (var iter in itemSlots.Where(s => s.ItemInstance.ItemData == item.ItemData && s.Count < slotCapacity))
            {
                int addAmount = Mathf.Min(slotCapacity - iter.Count, remaining);

                iter.Add(item, addAmount);
                remaining -= addAmount;

                OnChangedItemSlot?.Invoke(itemSlots.IndexOf(iter), iter);

                if (remaining <= 0)
                    return true;
            }

            foreach (var iter in itemSlots.Where(s => s.IsEmpty))
            {
                int addAmount = Mathf.Min(slotCapacity, remaining);
                
                iter.Add(item, addAmount);
                remaining -= addAmount;

                OnChangedItemSlot?.Invoke(itemSlots.IndexOf(iter), iter);

                if (remaining <= 0)
                    return true;
            }

            return true;
        }
    }

    private bool CanAdd(Item item, int amount)
    {
        if (item == null)
            return false;

        if (item is EquipmentItem)
        {
            var result = itemSlots.Where(s => s.IsEmpty);

            return result.Count() >= amount;
        }
        else
        {
            int space = itemSlots.Where(s => s.IsEmpty || s.ItemInstance.ItemData == item.ItemData).
                Sum(s => slotCapacity - s.Count);

            return space >= amount;
        }
    }

    private void Initialize()
    {
        itemSlots = new List<ItemSlot>();

        for (int i = 0; i < inventoryCapacity; i++)
        {
            itemSlots.Add(new ItemSlot());
        }
    }
}

[Serializable]
public class ItemSlot
{
    [field: SerializeField] public Item ItemInstance { get; private set; } = null;
    [field: SerializeField] public bool IsEmpty { get; private set; } = true;
    [field: SerializeField] public int Count { get; private set; } = 0;

    public void Add(Item item, int amount)
    {
        if (amount <= 0)
            return;

        ItemInstance = item;
        IsEmpty = false;
        Count += amount;
    }

    public void Remove(int amount)
    {

    }

    public ItemSlot()
    {
        ItemInstance = new NullItem(null);
    }
}

