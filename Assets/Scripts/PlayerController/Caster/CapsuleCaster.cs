using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCaster : ICastable
{
    private RaycastHit hitInfo;
    private Transform tr;
    private LayerMask targetLayers;
    private Vector3 origin = Vector3.zero;
    private Vector3 castDirection = Vector3.zero;
    private Vector3 capsuleTopPoint, capsuleBottomPoint;

    public Collider GetCollider() => hitInfo.collider;
    public float GetDistance() => hitInfo.distance;
    public RaycastHit GetHitInfo() => hitInfo;
    public Vector3 GetNormal() => hitInfo.normal;
    public Vector3 GetPoint() => hitInfo.point;
    public Rigidbody GetRigidbody() => hitInfo.rigidbody;
    public Transform GetTransform() => hitInfo.transform;
    public bool HasHit() => hitInfo.collider != null;

    public CapsuleCaster(Transform tr)
    {
        this.tr = tr;
    }

    public void SetLayerMask(LayerMask layers)
    {
        targetLayers = layers;
    }

    public void SetDirection(Vector3 direction)
    {
        castDirection = direction;
    }

    public void SetCastOrigin(Vector3 origin)
    {
        this.origin = tr.InverseTransformPoint(origin);
    }

    public void SetCapsulePosition(Vector3 top, Vector3 bottom)
    {
        capsuleTopPoint = tr.InverseTransformPoint(top);
        capsuleBottomPoint = tr.InverseTransformPoint(bottom);
    }

    public void Cast()
    {
        Vector3 worldOrigin = tr.TransformPoint(origin);
        Vector3 worldDirection = castDirection;

       // Physics.CapsuleCast()
    }
}
