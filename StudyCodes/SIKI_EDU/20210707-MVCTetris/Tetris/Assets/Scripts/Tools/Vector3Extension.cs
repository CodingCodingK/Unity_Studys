using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extension
{
    public static Vector2 Round(this Vector3 v3)
    {
        int x = Mathf.RoundToInt(v3.x);
        int y = Mathf.RoundToInt(v3.y);
        return new Vector2(x, y);
    }
}
