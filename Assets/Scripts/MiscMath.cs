using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MiscMath
{
    public static Vector3 Vector3Multiply(Vector3 lhs, Vector3 rhs)
    {
        lhs.x *= rhs.x;
        lhs.y *= rhs.y;
        lhs.z *= rhs.z;
        return lhs;
    }
    public static Vector3 Vector3Clamp(Vector3 vector, Vector3 min, Vector3 max)
    {
        vector.x = Mathf.Clamp(vector.x, min.x, max.x);
        vector.y = Mathf.Clamp(vector.y, min.y, max.y);
        vector.z = Mathf.Clamp(vector.z, min.z, max.z);
        return vector;
    }
}
