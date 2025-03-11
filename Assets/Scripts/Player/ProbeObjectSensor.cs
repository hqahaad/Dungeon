using System;
using System.Collections;
using UnityEngine;

public class ProbeObjectSensor : MonoBehaviour
{
    [SerializeField] private float castLength;
    [SerializeField] private LayerMask targetLayers;
    [SerializeField] private float sensorTick = 0.25f;

    private RaycastSensor sensor;
    private Collider colliderCache = null;

    public event Action<GameObject> OnSensorDetected = delegate { };

    void Awake()
    {
        sensor = new(transform);
    }

    void OnEnable()
    {
        var delay = new WaitForSeconds(sensorTick);

        StartCoroutine(UpdateSensor(delay));
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * castLength, Color.magenta);
    }

    private IEnumerator UpdateSensor(WaitForSeconds delay)
    {
        while (true)
        {
            var direction = transform.forward;

            sensor.WithCastLength(castLength).
                WithCastPoint(transform.position).
                WithCastDirection(direction).
                WithCastLayers(targetLayers).
                WithQueryTriggerInteration(QueryTriggerInteraction.Collide).
                Cast();

            if (sensor.HasHit())
            {
                if (colliderCache == null || colliderCache != sensor.GetHitCollider())
                {
                    OnSensorDetected?.Invoke(sensor.GetHitCollider().gameObject);
                    colliderCache = sensor.GetHitCollider();
                }
            }
            else
            {
                OnSensorDetected?.Invoke(null);
                colliderCache = null;
            }

            yield return delay;
        }
    }
}
