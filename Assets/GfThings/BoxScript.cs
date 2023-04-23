using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BoxScript : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent<BoxScript> onToggle;
    public bool on;
    public GameObject tick;

    private void Start()
    {
        on = tick.gameObject.activeSelf;
    }

    public void click()
    {
        if(on)
        {
            on = false;
            tick.gameObject.SetActive(false);
        }
        else
        {
            on = true;
            tick.gameObject.SetActive(true);
        }

        onToggle.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        click();
    }
}

public class ToggleEvent : UnityEvent<bool> { }