using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class Inventory : MonoBehaviour
{
    public List<ItemSlot> itemSlots;

    private const int maxSlotCount = 3;

    void Awake()
    {
        Initialize();
    }

    public bool CanPickUpItem()
    {
        return true;
    }

    public void AddItem(ItemData item)
    {
        if (!CanPickUpItem())
            return;

        var emptySlot = itemSlots.FirstOrDefault(s => s.IsEmpty);

        if (emptySlot != null)
        {
            emptySlot.Add(item);
        }
    }

    public void ConsumeItem(ItemData item)
    {
        var targetSlot = itemSlots.FirstOrDefault(s => s.ItemData == item);

        if (targetSlot != null)
        {
            targetSlot.Consume();
        }
    }

    public void ConsumeItemByIndex(int index)
    {
        if (index >= maxSlotCount)
            return;

        if (!itemSlots[index].IsEmpty)
        {
            itemSlots[index].Consume();
        }
    }


    private void Initialize()
    {
        itemSlots = new List<ItemSlot>();

        for (int i = 0; i < maxSlotCount; i++)
        {
            itemSlots.Add(new ItemSlot());
        }
    }
}

[Serializable]
public class ItemSlot
{
    [field: SerializeField] public ItemData ItemData { get; private set; } = null;
    [field: SerializeField] public bool IsEmpty { get; private set; } = true;
    [field: SerializeField] public int Count { get; private set; } = 0;

    public void Add(ItemData item)
    {
        if (IsEmpty)
        {
            ItemData = item;
        }
        else if (ItemData == item)
        {
            Count += 1;
        }
    }

    public void Consume()
    {
        Count -= 1;

        if (Count <= 0)
        {
            ItemData = null;
            IsEmpty = true;
            Count = 0;
        }
    }
}

