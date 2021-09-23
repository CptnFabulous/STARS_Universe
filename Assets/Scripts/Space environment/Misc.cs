using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Misc
{
    

    public static string LogVector3Fine(Vector3 value)
    {
        return "(" + value.x + ", " + value.y + ", " + value.z + ")";
    }

    public static string LogVector2Fine(Vector2 value)
    {
        return "(" + value.x + ", " + value.y + ")";
    }
}
