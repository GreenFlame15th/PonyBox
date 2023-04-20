using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PonySciptable", menuName = "PonyBox/OneTime/PonySciptable", order = 0)]
public class PonyScriptable : ScriptableObject
{
    public float force;
    public float delay;
    public float selfRightingSpeed;
    public float backtoScreenForce;
    public float speedLimit;
    public float bouceLinency;
    public float minCollisonVelocty;
}
