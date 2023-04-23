using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnifiedPonyObject
{
    public PonyScriptable scriptable;
    public int numberOfSprites;
    public Sprite[] sprites;

    public List<PonyController> instances;
    public PonyGridElement ponyGridElement;

    public UnifiedPonyObject() { instances = new List<PonyController>(); }

    public void SetUp(List<Sprite> sprites, GameObject ponyGrid, GameObject ponyGridElmentPrefab, bool animate)
    {
        scriptable = PonyBoxManager.instance.ponyScriptable;
        this.sprites = sprites.ToArray();
        numberOfSprites = sprites.Count;
        PonyBoxManager.instance.ponies.Add(this);

        ponyGridElement = GameObject.Instantiate(ponyGridElmentPrefab, ponyGrid.transform).GetComponent<PonyGridElement>();
        ponyGridElement.SetUp(this, animate);
    }
    public void addPonyController(PonyController instance)
    {
        instances.Add(instance);
        ponyGridElement.count.text = instances.Count + "";
        instance.SetUp(this);
        
    }
    public void addInstance()
    {
        PonyBoxManager.instance.spriteMaker.SpawnPony(this);
    }
    public void enqueueInstance()
    {
        PonyBoxManager.instance.spriteMaker.EnqueueUPO(this);
    }
    public void destryoAllInstances()
    {
        foreach (PonyController pony in instances)
        {
            pony.Fold();
        }
        instances.Clear();
        ponyGridElement.count.text = "0";

    }
    public void destryoNInstances(int number)
    {
        if(instances.Count < number)
        {
            destryoAllInstances();
        }
        else
        {
            int i = 0;
            int count = instances.Count;
            while (i < number && count > 0)
            {
                instances[count - 1].Fold();
                instances.RemoveAt(count - 1);
                i++;
                count--;
            }

            ponyGridElement.count.text = instances.Count + "";
        }
    }
    public int canDestroy(int number) { return number < instances.Count ? number : instances.Count; }
    public void beGone()
    {
        foreach (PonyController pony in instances)
        {
            pony.Fold();
        }

        PonyBoxManager.instance.ponies.Remove(this);
        GameObject.Destroy(ponyGridElement.gameObject);
    }
}
