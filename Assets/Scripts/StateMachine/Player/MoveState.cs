using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(PlayerController controller) : base(controller)
    {
    }

    public override void Update()
    {
        base.Update();

        Debug.Log("Move State");
    }
}
