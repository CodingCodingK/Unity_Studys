using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    public const int MAX_ROWS = 23;
    public const int MAX_COLUMNS = 10;

    private Transform[,] map = new Transform[MAX_COLUMNS,MAX_ROWS];

    public bool IsValidMapPosition(Transform t)
    {
        foreach (Transform child in t)
        {
            if (child.tag != "Block") continue;
            
            Vector2 pos = child.position.Round();
            
            // 是否超出边界
            if (IsInsideMap(pos) == false) return false;
            // 是否有重叠
            if (map[(int) pos.x, (int) pos.y] != null) return false;
        }
        return true;
    }

    private bool IsInsideMap(Vector2 v2)
    {
        return v2.x >= 0 && v2.y >= 0 && v2.x < MAX_COLUMNS && v2.y < MAX_ROWS;
    }

    public void PlaceShape(Transform t)
    {
        foreach (Transform child in t)
        {
            if (child.tag != "Block") continue;
            Vector2 pos = child.position.Round();
            map[(int) pos.x, (int) pos.y] = child;
        }
    }
}
