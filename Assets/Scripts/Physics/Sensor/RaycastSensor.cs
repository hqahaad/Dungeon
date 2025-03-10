using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSensor : CastSensor
{
    private Vector3 castPosition;

    public RaycastSensor(Transform tr) : base(tr)
    {
    }

    public override void Cast()
    {
        Physics.Raycast(castPosition, castDirection, out castHit, castLength, targetLayers);
    }

    public RaycastSensor WithCastPoint(Vector3 pos)
    {
        castPosition = pos;

        return this;
    }

    public RaycastSensor WithCastDirection(Vector3 direction)
    {
        castDirection = direction.normalized;

        return this;
    }

    public RaycastSensor WithCastLength(float length)
    {
        castLength = length;

        return this;
    }

    public RaycastSensor WithCastLayers(LayerMask layers)
    {
        targetLayers = layers;

        return this;
    }

    public RaycastSensor WithQueryTriggerInteration(QueryTriggerInteraction queryTriggerInteraction)
    {
        this.queryTriggerInteraction = queryTriggerInteraction;

        return this;
    }
}
