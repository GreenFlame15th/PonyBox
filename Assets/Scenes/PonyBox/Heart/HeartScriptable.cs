using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeartScriptable", menuName = "PonyBox/OneTime/HeartScriptable", order = 0)]
public class HeartScriptable : ScriptableObject
{
    public float animationDistance;
    public float animationTime;
    public AnimationCurve travelCurve;
    public AnimationCurve fade;
    public float scaleMiltiplaier;
    public float minhartSize;
}
