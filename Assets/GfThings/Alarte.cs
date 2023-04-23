using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Alarte : MonoBehaviour
{
    private Funcion funcion;
    public Text alarteText;
    public Text titleText;

    public void Invoke(string title, string alarte)
    {
        if (!gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            titleText.text = title;
            alarteText.text = alarte;
        }
    }
}
