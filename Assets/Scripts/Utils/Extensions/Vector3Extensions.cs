using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 ExtractDotVector(this Vector3 vector, Vector3 direction)
    {
        direction.Normalize();

        return direction * Vector3.Dot(vector, direction);
    }

    public static Vector3 RemoveDotVector(this Vector3 vector, Vector3 direction)
    {
        direction.Normalize();

        return vector - direction * Vector3.Dot(vector, direction);
    }
    
    public static float GetDotProduct(this Vector3 vector, Vector3 direction)
    {
        return Vector3.Dot(vector, direction.normalized);
    }
}
