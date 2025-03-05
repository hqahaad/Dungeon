using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class CharacterMotor : MonoBehaviour
{
    [Header("Collider Settings")]
    [SerializeField, Range(0f, 1f)] private float stepHeightRatio = 0.1f;
    [SerializeField] private float colliderHeight = 2f;
    [SerializeField] private float colliderRadius = 0.5f;
    [SerializeField] private Vector3 colliderOffset = Vector3.zero;

    [Header("Cast Settings")]
    [SerializeField] private float skinWidth = 0.01f;

    [Header("Layers Settings")]
    [SerializeField] private LayerMask targetLayers;

    [Header("Gravity Settings")]
    [SerializeField] private bool useGravity = true;
    [SerializeField] private float gravity = 30f;
    [SerializeField] private float gravityFriction = 100f;
    [SerializeField] private float slideGravity = 5f;

    [Header("Slope Setting")]
    [SerializeField, Range(0f, 89f)] private float slopeLimit = 60f;

    private Rigidbody body;
    private Transform tr;
    private CapsuleCollider col;

    [Header("Debug")]
    private bool isGrounded = false;
    private bool isSloped = false;
    private bool isSteepSloped = false;

    private Vector3 currentGroundAdjustVelocity = Vector3.zero;
    private Vector3 momentum, inputVelocity, savedVelocity = Vector3.zero;

    public float StepHeightFromCapsule => (colliderHeight - col.height);
    public Vector3 CapsuleTopPoint => col.bounds.center + new Vector3(0f, (col.height * 0.5f * tr.localScale.y) - col.radius * tr.localScale.x, 0f);
    public Vector3 CapsuleBottomPoint => col.bounds.center - new Vector3(0f, (col.height * 0.5f * tr.localScale.y) - col.radius * tr.localScale.x, 0f);

    void Awake()
    {
        SetUp();
    }

    void OnValidate()
    {
        RecalculateColliderDimensions();
    }

    void FixedUpdate()
    {
        DetectingGround();
        HandleMomentum();

        //SetVelocity(Vecto);
        SetVelocity(momentum);
    }

    public void SetVelocity(Vector3 velocity) => body.velocity = velocity + currentGroundAdjustVelocity;

    public void DetectingGround()
    {
        currentGroundAdjustVelocity = Vector3.zero;
        isGrounded = false;

        const float safetyDistanceFactor = 0.001f;

        float castRadius = colliderRadius * tr.localScale.x;
        float castLength = StepHeightFromCapsule * tr.localScale.x + safetyDistanceFactor;

        var cast = Physics.CapsuleCast(CapsuleTopPoint, CapsuleBottomPoint, castRadius - skinWidth,
            -tr.up, out var hit, castLength, targetLayers);

        if (cast)
        {
            isGrounded = true;

            float distance = hit.distance;
            float distanceToGo = StepHeightFromCapsule - distance;

            currentGroundAdjustVelocity = tr.up * (distanceToGo / Time.fixedDeltaTime);
        }
    }

    public void HandleMomentum()
    {
        Vector3 verticalMomentum = momentum.ExtractDotVector(tr.up);
        Vector3 horizontalMomentum = momentum - verticalMomentum;

        verticalMomentum -= tr.up * (gravity * Time.deltaTime);

        if (isGrounded && verticalMomentum.GetDotProduct(tr.up) < 0f)
        {
            verticalMomentum = Vector3.zero;
        }

        if (!isGrounded)
        {

        }

        momentum = horizontalMomentum + verticalMomentum;
    }

    private void SetUp()
    {
        tr = transform;
        body = GetComponent<Rigidbody>();
        col = GetComponent<CapsuleCollider>();

        body.freezeRotation = true;
        body.useGravity = false;
    }

    private void RecalculateColliderDimensions()
    {
        if (!col) SetUp();

        col.height = colliderHeight * (1f - stepHeightRatio);
        col.radius = colliderRadius;
        col.center = colliderOffset * colliderHeight + new Vector3(0f, (colliderHeight - col.height) * 0.5f, 0f);

        if (col.height * 0.5f < col.radius)
        {
            col.radius = col.height * 0.5f;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(col.bounds.center, 0.1f);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(col.bounds.center + new Vector3(0f, -col.height * 0.5f, 0f),
            col.bounds.center + new Vector3(0f, -col.height * 0.5f, 0f) + new Vector3(0f, -StepHeightFromCapsule, 0f));

        Gizmos.DrawSphere(CapsuleTopPoint, 0.2f);
        Gizmos.DrawSphere(CapsuleBottomPoint, 0.1f);
    }
}


public struct GroundInfo
{
    public RaycastHit rayHit;

    public Vector3 GetNormal() => rayHit.normal;
    public float GetDistance() => rayHit.distance;
    public Collider GetCollider() => rayHit.collider;
    public bool HasHit() => rayHit.collider != null;
    public Vector3 GetPoint() => rayHit.point;
    public Rigidbody GetRigidbody() => rayHit.rigidbody;
}

public class CharacterState
{
    public bool isGrounded;
    public bool isSloped;
    public bool isSteepSloped;
}