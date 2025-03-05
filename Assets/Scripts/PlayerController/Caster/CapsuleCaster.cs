using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCaster : ICastable
{
    private RaycastHit hitInfo;
    private Transform tr;
    private Vector3 origin = Vector3.zero;
    private Vector3 CastDirection = Vector3.zero;

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

    public void A() { }

    public void Cast()
    {
        Vector3 worldOrigin = tr.TransformPoint(Vector3.zero);

       // Physics.CapsuleCast()
    }
}
