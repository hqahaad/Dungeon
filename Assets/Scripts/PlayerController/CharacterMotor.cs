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
    [SerializeField] private float castLength = 1f;
    [SerializeField] private float skinWidth = 0.01f;

    [Header("Layers")]
    [SerializeField] private LayerMask targetLayers;

    private Rigidbody body;
    private Transform tr;
    private CapsuleCollider col;

    private bool isGrounded = false;
    private float baseSensorRange;
    private Vector3 currentGroundAdjustVelocity = Vector3.zero;
    private int currentLayer;

    private CapsuleCaster sensor;

    public float StepHeightFromCapsule => (colliderHeight - col.height);

    void Awake()
    {
        SetUp();
    }

    void OnValidate()
    {
        RecalculateColliderDimensions();
    }

    public void DetectingGround()
    {
        sensor ??= new CapsuleCaster(tr);

        sensor.Cast();

        currentGroundAdjustVelocity = Vector3.zero;

        isGrounded = sensor.HasHit();
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
