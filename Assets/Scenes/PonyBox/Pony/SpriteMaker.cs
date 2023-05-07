using B83.Image.GIF;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpriteMaker : MonoBehaviour
{
    public int sprtieCount;
    public GameObject ponyPrefab;
    public float spawnDelay;
    public Queue<PonyController> ponyQueue = new Queue<PonyController>();
    private GameObjectQueue ponyGOQueue;

    public GameObject ponyGrid;
    public GameObject ponyGridElmentPrefab;

    public GameObject animatsionConfigPreFab;

    public void makePony(byte[] fileData, bool gif)
    {
        AnimatsionConfig config = GameObject.Instantiate(
            animatsionConfigPreFab,
            PonyBoxManager.instance.popUps.transform
            ).GetComponent<AnimatsionConfig>();
        config.Invoke(fileData, gif);
    }

    public void StartSetUp()
    {
        ponyGOQueue = new GameObjectQueue(ponyPrefab);
        StartCoroutine(ponySpanwer());
    }
    private IEnumerator ponySpanwer()
    {
        while(true)
        {
            while(ponyQueue.TryDequeue(out PonyController pony))
            {
                if(pony.inSpawningQueue)
                {
                    pony.inSpawningQueue = false;
                    pony.gameObject.SetActive(true);
                    pony.ponyCollider.enabled = true;
                    pony.InitPush();
                    yield return new WaitForSeconds(spawnDelay);
                }
                else
                {
                    pony.Fold();
                }
            }
            yield return new WaitForSeconds(spawnDelay);
        }

    }

    public void SpawnPony(UnifiedPonyObject upo)
    {
        PonyController pony = MakeInstance(upo);
        pony.InitPush();
        pony.gameObject.SetActive(true);
    }

    public PonyController MakeInstance(UnifiedPonyObject upo)
    {
        //get randome spawn point
        Queueable ponyGO = ponyGOQueue.GetGameObject(Vector3.one, Quaternion.identity, this.transform);
        PonyController pony = ponyGO.GetComponent<PonyController>();
        upo.addPonyController(pony);
        return pony;
    }

    public void EnqueueUPO(UnifiedPonyObject upo)
    {
        PonyController pony = MakeInstance(upo);
        pony.ponyCollider.enabled = false;
        pony.inSpawningQueue = true;
        ponyQueue.Enqueue(pony);
        pony.gameObject.SetActive(false);
    }

    public void ClearQueue()
    {
        ponyQueue.Clear();
    }
}

