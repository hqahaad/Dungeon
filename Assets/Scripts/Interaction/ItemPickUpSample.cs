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

        player.Inventory.AddItem(items[Random.Range(0, items.Count)]);
    }
}
