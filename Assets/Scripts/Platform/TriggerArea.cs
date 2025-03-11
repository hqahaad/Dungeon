using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    private readonly List<Rigidbody> rigidbodies = new();

    public IReadOnlyList<Rigidbody> Rigidbodies => rigidbodies;

    void OnTriggerEnter(Collider col)
    {
        if (col.attachedRigidbody != null)
        {
            Debug.Log("asdf");
            rigidbodies.Add(col.attachedRigidbody);
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.attachedRigidbody != null)
        {
            rigidbodies.Remove(col.attachedRigidbody);
        }
    }
}