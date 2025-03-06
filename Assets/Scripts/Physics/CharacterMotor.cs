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

    [Header("Movement Setting")]
    [SerializeField] private float groundFriction = 100f;
    [SerializeField] private bool useAirFriction = true;
    [SerializeField] private float airFriction = 0.5f;
    [SerializeField] private float airControlRate = 0.5f;

    private Rigidbody body;
    private Transform tr;
    private CapsuleCollider col;

    private bool isGrounded = false;
    private bool isSloped = false;
    private bool isSteepSloped = false;
    private bool isUsingExtendedSensorRange = false;

    private CapsuleCastSensor sensor;
    private Vector3 currentGroundAdjustVelocity = Vector3.zero;
    private Vector3 momentum, inputVelocity, savedVelocity = Vector3.zero;
    private float savedMoveSpeed = 0f;

    public float StepHeightFromCapsule => (colliderHeight - col.height);
    public Vector3 CapsuleTopPoint => col.bounds.center + new Vector3(0f, (col.height * 0.5f * tr.localScale.y) - col.radius * tr.localScale.x, 0f);
    public Vector3 CapsuleBottomPoint => col.bounds.center - new Vector3(0f, (col.height * 0.5f * tr.localScale.y) - col.radius * tr.localScale.x, 0f);

    public void SetExtendSensorRange(bool isExtended) => isUsingExtendedSensorRange = isExtended;

    public bool IsGrounded() => isGrounded;
    public bool IsSloped() => isSloped;
    public bool IsSteepSloped() => isSteepSloped;

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

        Vector3 velocity = isGrounded ? inputVelocity : Vector3.zero;
        velocity += momentum;

        SetExtendSensorRange(isGrounded);
        SetVelocity(velocity);

        savedVelocity = velocity;
    }

    public void Move(Vector3 movementVelocity)
    {
        inputVelocity = movementVelocity;

        if (movementVelocity.magnitude > savedMoveSpeed)
        {
            savedMoveSpeed = movementVelocity.magnitude;
        }
    }

    public void Rotate(Vector3 moveDirection, float delta)
    {
        moveDirection.Normalize();

        if (moveDirection != Vector3.zero)
        {
            Quaternion target = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, delta);
        }
    }

    public void SetVelocity(Vector3 velocity) => body.velocity = velocity + currentGroundAdjustVelocity;

    public void DetectingGround()
    {
        sensor ??= new CapsuleCastSensor(tr);

        currentGroundAdjustVelocity = Vector3.zero;
        isGrounded = false;
        isSloped = false;
        isSteepSloped = false;

        const float safetyDistanceFactor = 0.001f;

        float castRadius = colliderRadius * tr.localScale.x - skinWidth;

        float baseLength = StepHeightFromCapsule * tr.localScale.y + safetyDistanceFactor;
        float castLength = isUsingExtendedSensorRange ?
            baseLength + colliderHeight * tr.localScale.y * stepHeightRatio :
            baseLength;

        sensor.WithCapsulePoint(CapsuleTopPoint, CapsuleBottomPoint).
            WithCastRadius(castRadius).
            WithCastDirection(-tr.up).
            WithCastLength(castLength).
            WithCastLayers(targetLayers).Cast();

        if (sensor.hasHit())
        {
            isGrounded = true;

            float distance = sensor.GetDistance();
            float distanceToGo = StepHeightFromCapsule - distance;

            currentGroundAdjustVelocity = tr.up * (distanceToGo / Time.fixedDeltaTime);

            Vector3 normal = sensor.GetNormal();

            if (normal != tr.up)
            {
                isSloped = true;

                if (Vector3.Angle(normal, tr.up) > slopeLimit)
                {
                    isSteepSloped = true;
                }
            }
        }
    }

    public void HandleMomentum()
    {
        Vector3 verticalMomentum = momentum.ExtractDotVector(tr.up);
        Vector3 horizontalMomentum = momentum - verticalMomentum;

        if (useGravity)
        {
            verticalMomentum -= tr.up * (gravity * Time.deltaTime);
        }

        if (isGrounded && verticalMomentum.GetDotProduct(tr.up) < 0f)
        {
            verticalMomentum = Vector3.zero;
        }

        if (!isGrounded)
        {
            //공중에서 가속 처리
            horizontalMomentum = inputVelocity;
            horizontalMomentum = Vector3.MoveTowards(horizontalMomentum, Vector3.zero, Time.deltaTime * airControlRate);
        }

        float friction = isGrounded || !useAirFriction ? groundFriction : airFriction;
        horizontalMomentum = Vector3.MoveTowards(horizontalMomentum, Vector3.zero, friction * Time.deltaTime);

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