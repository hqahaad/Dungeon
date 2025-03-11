using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpperSample : PickUp
{
    [SerializeField] private float jumpPower;

    public override void Interaction(PlayerController player) => player.Jump(jumpPower);
    public override string GetName() => "Jumpper";
    public override string GetDescription() => "Jump";
}
