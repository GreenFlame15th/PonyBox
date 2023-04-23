using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(false);

        if (Screen.fullScreen != true)
        {
            fullScreenToggle.click();
        }
    }

    public Slider camreSlider;
    public void UpdateScreenSize()
    {
        PonyBoxManager.instance.mainCamer.orthographicSize = (float)(System.Math.Pow(10f, camreSlider.value) / 10);
    }

    public Slider speedSlider;
    public void UpdateSpeed()
    {
        if (speedSlider.value == 0)
            Time.timeScale = 1;
        else if (speedSlider.value < 0)
            Time.timeScale = 1 / -speedSlider.value;
        else
            Time.timeScale = speedSlider.value;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            this.gameObject.SetActive(true);
        }
    }

    public BoxScript fullScreenToggle;

    public void ToggleFullScreen(BoxScript boxScript)
    {
        if (boxScript.on)
        {
            Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        }
        else
        { 
            Screen.fullScreenMode = FullScreenMode.Windowed;
        }
    }

    public void setSugarRush(BoxScript boxScript) { PonyBoxManager.instance.sugarRush = boxScript.on; }
    public void setHearts(BoxScript boxScript) { PonyBoxManager.instance.hearts = boxScript.on; }
    public void setWhirlpool(BoxScript boxScript) { PonyBoxManager.instance.whirlpool = boxScript.on; }
    public void setBorderMode(BorderMode borderMode) { PonyBoxManager.instance.borderMode = borderMode; }
    public void setBorderMode(int borderMode)
    {
        switch (borderMode)
        {
            case 0:
                setBorderMode(BorderMode.HARD);
                break;
            case 1:
                setBorderMode(BorderMode.SOFT);
                break;
            case 2:
                setBorderMode(BorderMode.NON);
                break;
            default:
                Debug.LogError("BorderMode " + borderMode + "not found");
                break;
        }
    }
    public void setPonyClickMode(int borderMode)
    {
        switch (borderMode)
        {
            case 0:
                PonyBoxManager.instance.ponyClickMode = ClickMode.Push;
                break;
            case 1:
                PonyBoxManager.instance.ponyClickMode = ClickMode.Drag;
                break;
            default:
                Debug.LogError("BorderMode " + borderMode + "not found");
                break;
        }
    }
}
