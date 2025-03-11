using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyState : BaseState
{
    public FlyState(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        playerController.PlayableAnimator.Play(playerController.jumpAnimation, 0.2f);

        //playerController.Motor.Jump(playerController.Stat.JumpPower);
    }
}
