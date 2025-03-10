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

    public event Action<GameObject> OnSensorHit = delegate { };

    void Awake()
    {
        sensor = new(transform);
    }

    void OnEnable()
    {
        var delay = new WaitForSeconds(sensorTick);

        StartCoroutine(UpdateSensor(delay));
    }

    private IEnumerator UpdateSensor(WaitForSeconds delay)
    {
        while (true)
        {
            var camDir = (transform.position - Camera.main.transform.position).normalized;

            sensor.WithCastLength(castLength).
                WithCastPoint(transform.position).
                WithCastDirection(camDir).
                WithCastLayers(targetLayers).
                WithQueryTriggerInteration(QueryTriggerInteraction.Collide).
                Cast();

            Debug.DrawLine(transform.position,
                transform.position + camDir * castLength, Color.green);

            if (sensor.HasHit())
            {
                if (colliderCache == null || colliderCache != sensor.GetHitCollider())
                {
                    OnSensorHit?.Invoke(sensor.GetHitCollider().gameObject);
                    colliderCache = sensor.GetHitCollider();
                }
            }
            else
            {
                OnSensorHit?.Invoke(null);
                colliderCache = null;
            }

            yield return delay;
        }
    }
}
