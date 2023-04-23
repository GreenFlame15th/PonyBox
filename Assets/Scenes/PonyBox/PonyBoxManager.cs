using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PonyBoxManager : MonoBehaviour
{
    public Vector2 screenBounds;
    public static PonyBoxManager instance;
    public List<UnifiedPonyObject> ponies;
    private ulong bounceCount = 0;
    public Text countDisplay;
    public GameObject heartPreFab;
    public GameObjectQueue heartQueue;
    public PonyScriptable ponyScriptable;

    //object refrence
    public Camera mainCamer;
    public SpriteMaker spriteMaker;
    public PonyManagmenMenu ponyManagmenMenu;
    public AreYouSurePopUp areYouSurePopUp;
    public Alarte alarte;

    //global for ponyBox
    public bool sugarRush;
    public bool hearts;
    public bool whirlpool;
    public BorderMode borderMode;
    public ClickMode ponyClickMode;

    public List<Texture2D> defoultPonies;

    private void Start()
    {
        ponies = new List<UnifiedPonyObject>();
        heartQueue = new GameObjectQueue(heartPreFab);

        instance = this;
        ponyManagmenMenu.UpdateSpawnDelay();
        spriteMaker.StartSetUp();

        loadDEfoultPonies();
    }

    public void loadDEfoultPonies()
    {
        for (int i = 0; i < defoultPonies.Count; i++)
        {
            UnifiedPonyObject upo = spriteMaker.MakePonyFromSprite(defoultPonies[i], 16, false);

            if (upo != null)
            {
                upo.enqueueInstance();
            }
            else
            {
                Debug.Log("Faild to load defoult pony #" + i);
            }
        }
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
