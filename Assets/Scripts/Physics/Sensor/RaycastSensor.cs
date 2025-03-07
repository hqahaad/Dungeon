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

    public RaycastSensor WidthCastPoint(Vector3 pos)
    {
        castPosition = pos;

        return this;
    }

    public RaycastSensor WidthCastDirection(Vector3 direction)
    {
        castDirection = direction.normalized;

        return this;
    }

    public RaycastSensor WidthCastLength(float length)
    {
        castLength = length;

        return this;
    }

    public RaycastSensor WithCastLayers(LayerMask layers)
    {
        targetLayers = layers;

        return this;
    }
}
