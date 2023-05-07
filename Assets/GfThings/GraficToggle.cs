using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GraficToggle : MonoBehaviour, IPointerClickHandler
{
    public bool on;
    public Sprite onGrafic;
    public Sprite offGrafic;
    public Image image;


    public void OnPointerClick(PointerEventData eventData)
    {
        if(on)
        {
            on = false;
            image.sprite = offGrafic;
        }
        else
        {
            on = true;
            image.sprite = onGrafic;
        }
    }

}
