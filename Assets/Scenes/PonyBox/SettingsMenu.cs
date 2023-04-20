using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
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
    public void Start()
    {
        if (Screen.fullScreen != true)
        {
            fullScreenToggle.click();
        }
    }

    public void ToggleFullScreen(BoxScript boxScript)
    {
        if (boxScript.on) Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        else Screen.fullScreenMode = FullScreenMode.Windowed;
    }

    public void setSugarRush(BoxScript boxScript) { PonyBoxManager.instance.sugarRush = boxScript.on; }
    public void setHearts(BoxScript boxScript) { PonyBoxManager.instance.hearts = boxScript.on; }
}
