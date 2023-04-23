using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class MiltySelectElement : MonoBehaviour, IPointerClickHandler
{
    public UnityEvent onToggle;
    public bool on;
    public GameObject tick;
    private MiltySelect boardConroller;
    public void OnPointerClick(PointerEventData eventData)
    {
        Click();
    }

    public void Click()
    {
        if(!on)
        {
            tick.SetActive(true);
            on = true;
            onToggle.Invoke();
            boardConroller.Toggled(gameObject.GetInstanceID());
        }
    }

    public void SetBoardConroller(MiltySelect boardConroller) { this.boardConroller = boardConroller; }

    public void off(int id)
    {
        if(on && id != this.gameObject.GetInstanceID())
        {
            tick.gameObject.SetActive(false);
            on = false;
        }
    }
}
