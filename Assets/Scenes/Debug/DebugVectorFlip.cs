using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugVectorFlip : MonoBehaviour
{
    public List<Vector2> vectors;

    void Start()
    {
        foreach (Vector2 vector in vectors)
        {
            Debug.Log(vector + " | " + vector.FinsSquere() + " | " + vector.Rotate90());
        }
    }

}
