using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Custom/SO/ItemData/Consumable")]
public class ConsumableItemData : ItemData
{
    [SerializeField] private StatType modifierStatType;
}