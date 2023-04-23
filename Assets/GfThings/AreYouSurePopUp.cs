using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AreYouSurePopUp : MonoBehaviour
{
    private Funcion funcion;
    public Text text;

    public void Invoke(string prompt, Funcion funcion)
    {
        if(!gameObject.activeSelf)
        {
            this.funcion = funcion;
            gameObject.SetActive(true);
            text.text = prompt;
        }
    }

    public void Yes()
    {
        funcion();
        gameObject.SetActive(false);
    }

    public void No()
    {
        gameObject.SetActive(false);
    }
}
