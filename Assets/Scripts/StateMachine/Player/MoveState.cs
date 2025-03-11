using UnityEngine;

public class MoveState : BaseState
{
    public MoveState(PlayerController controller) : base(controller)
    {
    }

    public override void OnEnter()
    {
        playerController.PlayableAnimator.Play(playerController.moveAnimation, 0.2f);
    }

    public override void Update()
    {
        var originDirection = playerController.GetMoveDirection();

        if (originDirection.magnitude == 0f)
            return;

        Transform cam = playerController.TargetCamera.transform;
        Vector3 project = Vector3.zero;
        var x = originDirection.x;
        var y = originDirection.y;

        if (cam != null)
        {
            var right = Vector3.ProjectOnPlane(cam.right, playerController.transform.up) * x;
            var forward = Vector3.ProjectOnPlane(cam.forward, playerController.transform.up) * y;

            project = right + forward;
        }
        else
        {
            project = originDirection;
        }

        playerController.Motor.Move(project * playerController.Stat.MoveSpeed);

        const float turnSpeed = 10f;

        if (playerController.Motor.IsGrounded())
        {
            playerController.Motor.Rotate(project, Time.deltaTime * turnSpeed);
        }
    }

    public override void OnExit()
    {
        //playerController.Motor.Move(Vector3.zero);
    }
}
