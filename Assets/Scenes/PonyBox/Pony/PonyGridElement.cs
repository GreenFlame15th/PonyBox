using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PonyGridElement : MonoBehaviour, IPointerClickHandler
{
    public StaticAnimatorImage display;
    public UnifiedPonyObject upo;
    public TMP_Text count;
    public bool selected;
    public GameObject selector;

    public void OnPointerClick(PointerEventData eventData)
    {
        PonyBoxManager.instance.ponyManagmenMenu.Toggle(this, !selected);
        Toggle(!selected);
    }

    public void Toggle(bool om)
    {
        selected = om;
        selector.SetActive(om);
    }

    public void SetUp(UnifiedPonyObject upo) 
    {
        this.upo = upo;
        display.StartAnimetsion(upo);
    }
}
