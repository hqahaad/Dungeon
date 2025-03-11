using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagerSample : PickUp
{
    [SerializeField] private float attackPower;

    public override string GetName() => "damage sample";
    public override string GetDescription() => $"damage Test -{attackPower}";

    public override void Interaction(PlayerController player) => player.GetDamage(attackPower);
}
