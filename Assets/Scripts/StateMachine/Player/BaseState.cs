using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : IState
{
    protected PlayerController playerController;

    public BaseState(PlayerController controller)
    {
        playerController = controller;
    }

    public virtual void FixedUpdate()
    {
    }

    public virtual void OnEnter()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void Update()
    {
    }
}
