using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    [SerializeField] protected string itemName;
    [SerializeField] protected string itemDesc;
    [SerializeField] protected Sprite itemIcon;
}
