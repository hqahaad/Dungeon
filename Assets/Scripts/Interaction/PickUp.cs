using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickUp : MonoBehaviour, IInteractable
{
    public abstract void Interaction(PlayerController player);
    public abstract string GetName();
    public abstract string GetDescription();
}
