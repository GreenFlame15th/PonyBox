using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PonyBoxManager : MonoBehaviour
{
    public Vector2 screenBounds;
    public static PonyBoxManager instance;
    public List<PonyController> ponies;
    private ulong bounceCount = 0;
    public Text countDisplay;
    public Camera mainCamer;
    public GameObject heartPreFab;
    public GameObjectQueue heartQueue;

    //global for ponyPox
    public bool sugarRush;
    public bool hearts;

    private void Start()
    {
        instance = this;
        heartQueue = new GameObjectQueue(heartPreFab);
    }

    // Update is called once per frame
    void Update()
    {
        screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    public void incrmentBouncCounter()
    {
        bounceCount++;
        countDisplay.text = "Corrner hits: " + bounceCount;
    }
}
