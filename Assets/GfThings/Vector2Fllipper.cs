using System.Collections;
using System.Collections.Generic;
using UnityEngine;

static class Vector2Fllipper
{
    public static Vector2 Rotate90(this Vector2 vector)
    {
        Vector2 toReturn = new Vector2();

        toReturn.x = -vector.y;
        toReturn.y = vector.x;

        return toReturn;
    }
}
