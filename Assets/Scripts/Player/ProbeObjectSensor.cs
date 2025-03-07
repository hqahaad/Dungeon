using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProbeObjectSensor : MonoBehaviour
{
    [SerializeField] private float castLength;
    [SerializeField] private LayerMask targetLayers;

    private RaycastSensor sensor;

    void Awake()
    {
        sensor = new(transform);
    }

    void Update()
    {
        var camDir = (transform.position - Camera.main.transform.position).normalized;

        sensor.WidthCastLength(castLength).
            WidthCastPoint(transform.position).
            WidthCastDirection(camDir).
            WithCastLayers(targetLayers).Cast();

        Debug.DrawLine(transform.position,
            transform.position + camDir * castLength, Color.green);

        if (sensor.HasHit())
        {
            Debug.Log(sensor.GetHitCollider().name);
        }
    }
}
