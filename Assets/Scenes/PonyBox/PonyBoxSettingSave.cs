using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class PonyBoxSettingSave
{
    public bool defoultsWhereLoaded = false;
    public bool uiWorn = true;

    public void save()
    {

        if(!Directory.Exists(Application.persistentDataPath))
        {
            Directory.CreateDirectory(Application.persistentDataPath);
        }

        string json = JsonUtility.ToJson(this);
        File.WriteAllText(Application.persistentDataPath + @"/settingsSave.json", json);

        Debug.Log("Game saved to " + Application.persistentDataPath + @"/settingsSave.json");
    }

    public static void load()
    {
        if (File.Exists(Application.persistentDataPath + @"/settingsSave.json"))
        {
            try
            {
                string json = File.ReadAllText(Application.persistentDataPath + @"/settingsSave.json");
                PonyBoxManager.instance.savedSettings = JsonUtility.FromJson<PonyBoxSettingSave>(json);
            }
            catch (System.Exception e)
            {

                Debug.LogError(e);
                PonyBoxManager.instance.savedSettings = new PonyBoxSettingSave();
                PonyBoxManager.instance.savedSettings.save();
            }
        }
        else
        {
            PonyBoxManager.instance.savedSettings = new PonyBoxSettingSave();
            PonyBoxManager.instance.savedSettings.save();
        }


    }
}
