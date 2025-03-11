using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUpSample : PickUp
{
    [SerializeField] private List<ItemData> items;

    public override string GetDescription() => "get random items (sample)";

    public override string GetName() => "Item Box";

    public override void Interaction(PlayerController player)
    {
        if (items.Count == 0)
            return;

        var newItem = items[Random.Range(0, items.Count)];

        if (newItem is EquipmentItemData)
        {
            EquipmentItem newEquipment = new EquipmentItem(newItem);

            player.Inventory.AddItem(new EquipmentItem(newItem), 1);
        }
        else
        {
            player.Inventory.AddItem(new ConsumableItem(newItem), Random.Range(0, 50));
        }
    }
}
