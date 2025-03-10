using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : BaseState
{
    public JumpState(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        playerController.PlayableAnimator.Play(playerController.jumpAnimation, 0.2f);

        playerController.Motor.Jump(7f);
    }
}
