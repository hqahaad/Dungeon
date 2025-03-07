using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;
    [SerializeField]
    private float jumpPower = 7f;

    private CharacterMotor motor;
    private AnimationSystem animSystem;
    private Camera targetCamera;

    StateMachine stateMachine = new();

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
        targetCamera = Camera.main;

        SetUpState();
    }

    void Update()
    {
        stateMachine?.Update();

        var x = Input.GetAxis("Horizontal");
        var z = Input.GetAxis("Vertical");

        Vector3 movementVector = Vector3.zero;

        if (targetCamera != null)
        {
            var right = Vector3.ProjectOnPlane(targetCamera.transform.right, transform.up).normalized * x;
            var forward = Vector3.ProjectOnPlane(targetCamera.transform.forward, transform.up).normalized * z;

            movementVector = right + forward;
        }

        motor.Move(movementVector.normalized * moveSpeed);

        if (motor.IsGrounded())
        {
            motor.Rotate(movementVector, Time.deltaTime * 10f);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            motor.Jump(jumpPower);
        }
    }

    void FixedUpdate()
    {
        stateMachine?.FixedUpdate();    
    }

    #region State
    private void SetUpState()
    {
        var idleState = new IdleState(this);
        var moveState = new MoveState(this);
        var jumpState = new JumpState(this);

        At(idleState, moveState, new FuncPredicate(() => !motor.IsGrounded()));

        At(moveState, idleState, new FuncPredicate(() => motor.IsGrounded()));

        stateMachine.SetState(idleState);
    }

    private void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    private void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
    #endregion
}
