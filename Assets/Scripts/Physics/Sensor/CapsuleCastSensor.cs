using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CapsuleCastSensor : CastSensor
{
    private float castRadius;
    private Vector3 topPoint, bottomPoint;

    public CapsuleCastSensor(Transform tr) : base(tr)
    {
    }

    public override void Cast()
    {
        Physics.CapsuleCast(topPoint, bottomPoint, castRadius, castDirection,
            out castHit, castLength, targetLayers, QueryTriggerInteraction.Ignore);
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

    public CapsuleCastSensor WithQueryTriggerInteration(QueryTriggerInteraction queryTriggerInteraction)
    {
        this.queryTriggerInteraction = queryTriggerInteraction;

        return this;
    }
}
