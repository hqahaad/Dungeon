using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CastSensor
{
    protected Transform tr;
    protected RaycastHit castHit;
    protected float castLength;
    protected Vector3 castDirection;
    protected LayerMask targetLayers;
    protected QueryTriggerInteraction queryTriggerInteraction;

    public virtual Vector3 GetPoint() => castHit.point;
    public virtual Vector3 GetNormal() => castHit.normal;
    public virtual float GetDistance() => castHit.distance;
    public virtual Collider GetHitCollider() => castHit.collider;
    public virtual Rigidbody GetHitRigidbody() => castHit.rigidbody;
    public virtual Transform GetTransform() => castHit.transform;
    public virtual bool HasHit() => castHit.collider != null;

    public CastSensor(Transform tr)
    {
        this.tr = tr;
    }

    public abstract void Cast();
}
