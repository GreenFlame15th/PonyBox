using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpriteMaker : MonoBehaviour
{
    public int sprtieCount;
    public GameObject ponyPreFab;
    public string filePath;
    public float spawnDelay;

    private void Start()
    {
        StartCoroutine(ponySpanwer());
    }

    private IEnumerator ponySpanwer()
    {
        Texture2D tex = null;


        string[] fileNames = Directory.GetFiles(filePath);

        // Loop through each file and read its content
        foreach (string fileName in fileNames)
        {
            if (fileName.EndsWith(".png"))
            {
                byte[] fileData;
                //geting texture
                fileData = File.ReadAllBytes(fileName);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                                         //formant to prevent bluer
                tex.filterMode = FilterMode.Point;
                //get randome spawn point
                Vector3 spawnPoint;
                if (Random.Range(0, 2) > 0.5)
                { spawnPoint = new Vector3(1.1f, 1.1f, 0); }
                else { spawnPoint = new Vector3(-0.1f, -0.1f, 0); }
                if (Random.Range(0, 2) > 0.5)
                { spawnPoint.x = Random.Range(-0.1f, 1.1f); }
                else { spawnPoint.y = Random.Range(-0.1f, 1.1f); }
                spawnPoint = PonyBoxManager.instance.mainCamer.ViewportToWorldPoint(spawnPoint);
                spawnPoint.z = 0;
                //animate pony
                GameObject ponyGO = GameObject.Instantiate(ponyPreFab, spawnPoint, Quaternion.identity, this.transform);
                if (ponyGO.TryGetComponent<PonyController>(out PonyController pony))
                {
                    pony.StartAnimetsion(tex, sprtieCount);
                }
                else
                {
                    GameObject.Destroy(ponyGO);
                }

                yield return new WaitForSeconds(spawnDelay);
            }
        }

    }
}
