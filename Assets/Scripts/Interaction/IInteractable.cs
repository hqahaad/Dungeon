using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    void Interaction(PlayerController player);
    string GetName();
    string GetDescription();
}
