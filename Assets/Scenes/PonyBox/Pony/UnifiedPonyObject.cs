
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class UnifiedPonyObject
{
    public PonyScriptable scriptable;
    public int numberOfSprites;
    public Sprite[] sprites;

    public List<PonyController> instances;
    public PonyGridElement ponyGridElement;
    public AnimatsionGuide guide;

    public UnifiedPonyObject()
    {
        instances = new List<PonyController>();
        scriptable = PonyBoxManager.instance.ponyScriptable;
    }

    public void ReciveSprites(List<Sprite> sprites)
    {
        this.sprites = sprites.ToArray();
        numberOfSprites = sprites.Count;
    }

    public void ReadyToGo()
    {
        PonyBoxManager.instance.ponies.Add(this);

        ponyGridElement = GameObject.Instantiate(PonyBoxManager.instance.spriteMaker.ponyGridElmentPrefab, PonyBoxManager.instance.spriteMaker.ponyGrid.transform).GetComponent<PonyGridElement>();
        ponyGridElement.SetUp(this);
        ponyGridElement.display.transform.localScale = new Vector3(guide.scaleX, guide.scaleY, 1);
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
