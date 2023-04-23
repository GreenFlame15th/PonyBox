using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pointer : MonoBehaviour
{
   public PonyController pony;
    public GameObject ninie;

    private void FixedUpdate()
    {
        this.transform.position = pony.getScreenCenterDirecion()/2;
        ninie.transform.position = pony.getScreenCenterDirecion().Rotate90() / 2;
    }

}
