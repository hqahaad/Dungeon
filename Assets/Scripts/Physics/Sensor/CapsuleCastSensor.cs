using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCastSensor
{
    private Transform tr;

    private float castLength;
    private float castRadius;
    private Vector3 topPoint, bottomPoint;
    private Vector3 castDirection;
    private LayerMask targetLayers;

    public RaycastHit raycastHit;

    public Vector3 GetPoint() => raycastHit.point;
    public Vector3 GetNormal() => raycastHit.normal;
    public float GetDistance() => raycastHit.distance;
    public Collider GetHitCollider() => raycastHit.collider;
    public Rigidbody GetHitRigidbody() => raycastHit.rigidbody;
    public Transform GetTransform() => raycastHit.transform;
    public bool hasHit() => raycastHit.collider != null;

    public CapsuleCastSensor(Transform tr)
    {
        this.tr = tr;
    }

    public void Cast()
    {
        Physics.CapsuleCast(topPoint, bottomPoint, castRadius, castDirection,
            out raycastHit, castLength, targetLayers);
    }

    public CapsuleCastSensor WithCapsulePoint(Vector3 top, Vector3 bottom)
    {
        topPoint = top;
        bottomPoint = bottom;

        return this;
    }

    public CapsuleCastSensor WithCastDirection(Vector3 direction)
    {
        castDirection = direction.normalized;

        return this;
    }

    public CapsuleCastSensor WithCastLength(float length)
    {
        castLength = length;

        return this;
    }

    public CapsuleCastSensor WithCastRadius(float radius)
    {
        castRadius = radius;

        return this;
    }

    public CapsuleCastSensor WithCastLayers(LayerMask layers)
    {
        targetLayers = layers;

        return this;
    }
}
