using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public ItemData ItemData { get; private set; }

    public Item(ItemData itemData) => ItemData = itemData;
}

public class ConsumableItem : Item
{
    public ConsumableItem(ItemData itemData) : base(itemData)
    {

    }
}

public class EquipmentItem : Item
{
    public EquipmentItem(ItemData itemData) : base(itemData)
    {

    }
}

public class NullItem : Item
{
    public NullItem(ItemData itemData) : base(itemData)
    {
    }
}