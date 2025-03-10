using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : BaseState
{
    public IdleState(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        playerController.PlayableAnimator.Play(playerController.idleAnimation, 0.2f);
    }
}
