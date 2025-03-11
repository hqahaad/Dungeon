using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

[RequireComponent(typeof(CharacterMotor))]
public class PlayerController : MonoBehaviour
{
    [Header("Animations")]
    //스테이트들에 생성자에 넣어주는 방식으로 수정
    [SerializeField] public AnimationClip idleAnimation;
    [SerializeField] public AnimationClip moveAnimation;
    [SerializeField] public AnimationClip jumpAnimation;

    [Header("SO Events")]
    [SerializeField] private InteractableGameEvent interactionEvent;

    //Reference
    private CharacterMotor motor;
    private Camera targetCamera;
    private PlayableAnimator playableAnimator;
    private InputReader inputReader;
    private CharacterStat stat;
    private ProbeObjectSensor probeSensor;
    private Inventory inventory;

    //Property
    public CharacterMotor Motor => motor;
    public Camera TargetCamera => targetCamera;
    public PlayableAnimator PlayableAnimator => playableAnimator;
    public CharacterStat Stat => stat;
    public Inventory Inventory => inventory;

    private StateMachine stateMachine = new();

    private IInteractable currentInteractable;

    void Awake()
    {
        motor = GetComponent<CharacterMotor>();
        targetCamera = Camera.main;
        playableAnimator = GetComponent<PlayableAnimator>();
        inputReader = GetComponent<InputReader>();
        stat = GetComponent<CharacterStat>();
        probeSensor = GetComponent<ProbeObjectSensor>();
        inventory = GetComponent<Inventory>();
    }

    void Start()
    {
        //Sample
        Cursor.visible = false;

        SetUpState();

        probeSensor.OnSensorDetected += (go) =>
        {
            if (go == null)
            {
                interactionEvent?.Raise(null);
                return;
            }

            currentInteractable = go.TryGetComponent(out IInteractable interactable) ? interactable : null;
            interactionEvent?.Raise(currentInteractable);
        };
    }

    void Update()
    {
        stateMachine?.Update();

        if (Input.GetKey(KeyCode.C))
        {
            Stat.Hp -= 0.1f;
        }
    }

    void FixedUpdate()
    {
        stateMachine?.FixedUpdate();
    }

    public Vector2 GetMoveDirection()
    {
        return inputReader.GetMoveDirection();
    }

    public void Interaction()
    {
        currentInteractable?.Interaction(this);
    }

    public void GetDamage(float damage)
    {
        Stat.Hp -= damage;
    }

    #region State
    private void SetUpState()
    {
        var idleState = new IdleState(this);
        var moveState = new MoveState(this);
        var jumpState = new JumpState(this);

        At(idleState, moveState, new FuncPredicate(() => inputReader.IsMoveKeyPressed));
        At(idleState, jumpState, new FuncPredicate(() => motor.IsGrounded() && inputReader.IsJumpKeyPressed));

        At(moveState, idleState, new FuncPredicate(() => !inputReader.IsMoveKeyPressed));
        At(moveState, jumpState, new FuncPredicate(() => motor.IsGrounded() && inputReader.IsJumpKeyPressed));

        At(jumpState, idleState, new FuncPredicate(() => motor.IsGrounded() && !inputReader.IsMoveKeyPressed));
        At(jumpState, moveState, new FuncPredicate(() => motor.IsGrounded() && inputReader.IsMoveKeyPressed));

        stateMachine.SetState(idleState);
    }

    private void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
    private void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);
    #endregion
}
