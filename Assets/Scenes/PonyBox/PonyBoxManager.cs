using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
    public bool loadingPoniesDone;

    //object refrence
    public Camera mainCamer;
    public SpriteMaker spriteMaker;
    public PonyManagmenMenu ponyManagmenMenu;
    public AreYouSurePopUp areYouSurePopUp;
    public Alarte alarte;
    public GameObject popUps;
    public DefoultPonies defoultPonies;

    //global for ponyBox
    public bool sugarRush;
    public bool hearts;
    public bool whirlpool;
    public BorderMode borderMode;
    public ClickMode ponyClickMode;
    public List<AnimatsionConfig> animatsionConfigs;
    public PonyBoxSettingSave savedSettings;
    
    public void SafeCurrentAsDefount()
    {
        defoultPonies.list = new List<AnimatsionGuide>();
        for (int i = 0; i < ponies.Count; i++)
        {
            defoultPonies.list.Add(ponies[i].guide);
        }
    }

    private void Start()
    {
        ponies = new List<UnifiedPonyObject>();
        heartQueue = new GameObjectQueue(heartPreFab);
        instance = this;
        PonyBoxSettingSave.load();

        ponyManagmenMenu.UpdateSpawnDelay();
        spriteMaker.StartSetUp();

        loadSavedPonies();

        if(!savedSettings.defoultsWhereLoaded)
        {
            loadDEfoultPonies();
            savedSettings.defoultsWhereLoaded = true;
            savedSettings.save();
        }
        ponyManagmenMenu.spawnFromList(1, ponies);


    }

    public void loadDEfoultPonies()
    {
        spriteMaker.makePonyFromList(defoultPonies.list);
    }

    public void loadSavedPonies()
    {
        int i = 0;
        while(File.Exists(Application.persistentDataPath + ponyScriptable.path + i + ponyScriptable.format))
        {
            string json = File.ReadAllText(Application.persistentDataPath + ponyScriptable.path + i + ponyScriptable.format);
            spriteMaker.makePonyFromGuide(JsonUtility.FromJson<AnimatsionGuide>(json));
			
			i++;

            if(i == int.MaxValue)
            {
                throw (new Exception("Too many files"));
            }
        }

        loadingPoniesDone = true;
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
