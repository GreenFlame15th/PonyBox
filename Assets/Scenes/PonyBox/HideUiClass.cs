using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HideUiClass : MonoBehaviour
{
    public bool on;
    public GameObject[] ui;
    public bool wasClicked;
    public GameObject ani_ui;
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!wasClicked)
            {
                wasClicked = true;
                Toggle();
            }
        }
        else
        {
            wasClicked = false;
        }
    }

    public void Toggle()
    {
        if(PonyBoxManager.instance.savedSettings.uiWorn)
        {
            PonyBoxManager.instance.areYouSurePopUp.Invoke("You are about to hide UI, you can bring it back by pressing escape (back on mobile) or tapping the rightmost part of the screen.", () =>
            {
                PonyBoxManager.instance.savedSettings.uiWorn = false;
                PonyBoxManager.instance.savedSettings.save();
                Toggle();
            });
        }
        else
        {
            on = !on;
            for (int i = 0; i < ui.Length; i++)
            {
                ui[i].SetActive(on);
            }
            ani_ui.SetActive(!on);
        }
    }
}
