using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Vector2Squere
{
    UP_LEFT,
    UP_RIGHT,
    DOWN_LEFT,
    DOWN_RIGHT,
    ZERO

}

static class Vector2SquereFiner
{
    public static Vector2Squere FinsSquere(this Vector2 vector)
    {
        if (vector.x == 0 && vector.y == 0)
            return Vector2Squere.ZERO;

        if (vector.x > 0)
        {
            if (vector.y > 0)
            {
                return Vector2Squere.UP_RIGHT;
            }
            else
            {
                return Vector2Squere.DOWN_RIGHT;
            }
        }
        else
        {
            if (vector.y > 0)
            {
                return Vector2Squere.UP_LEFT;
            }
            else
            {
                return Vector2Squere.DOWN_LEFT;
            }
        }
    }

}