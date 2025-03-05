using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICastable
{
    Vector3 GetPoint();
    Vector3 GetNormal();
    float GetDistance();
    Collider GetCollider();
    Rigidbody GetRigidbody();
    Transform GetTransform();
    bool HasHit();
    RaycastHit GetHitInfo();

    void Cast();
}
