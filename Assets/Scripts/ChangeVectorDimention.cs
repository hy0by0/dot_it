using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ChangeVectorDimention
{
   public static Vector3 ChangeDimention(Vector3 v)
    {
        return new Vector2(v.x, v.y);
    }
}
