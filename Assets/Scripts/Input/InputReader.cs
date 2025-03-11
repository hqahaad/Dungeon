using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour, UserActions.IPlayerActions
{
    private UserActions input;

    [SerializeField] private UnityEvent<Vector2> OnMoveEvent;
    [SerializeField] private UnityEvent OnJumpEvent;
    [SerializeField] private UnityEvent OnInteractionEvent;
    [SerializeField] private UnityEvent OnInventoryEvent;

    public bool IsMoveKeyPressed { get; private set; }
    public bool IsJumpKeyPressed { get; private set; }
    public bool IsInteractionKeyPressed { get; private set; }

    public Vector2 GetMoveDirection() => input.Player.Move.ReadValue<Vector2>();

    void Awake()
    {
        input = new UserActions();
        input.Player.SetCallbacks(this);
    }

    void OnEnable()
    {
        input.Enable();
    }

    void OnDisable()
    {
        input.Disable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        OnMoveEvent?.Invoke(context.ReadValue<Vector2>());

        if (context.phase == InputActionPhase.Started)
        {
            IsMoveKeyPressed = true;   
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            IsMoveKeyPressed = false;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        OnJumpEvent?.Invoke();

        if (context.phase == InputActionPhase.Started)
        {
            IsJumpKeyPressed = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            IsJumpKeyPressed = false;
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            IsInteractionKeyPressed = true;
            OnInteractionEvent?.Invoke();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            IsInteractionKeyPressed = false;
        }
    }

    public void OnOnInventory(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            OnInventoryEvent?.Invoke();
        }
    }
}
