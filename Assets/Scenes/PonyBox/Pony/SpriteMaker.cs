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

    public void MakePonyFromPng(byte[] fileData, int numberOfSprites = 16, bool animatePonyGridElment = true)
    {
        Texture2D
        tex = new Texture2D(2, 2);
        tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                                 //formant to prevent bluer
        MakePonyFromSprite(tex, numberOfSprites, animatePonyGridElment);

    }

    public UnifiedPonyObject MakePonyFromSprite(Texture2D tex, int numberOfSprites = 16, bool animatePonyGridElment = true)
    {
        tex.filterMode = FilterMode.Point;

        int spriteWidth = tex.width / numberOfSprites;

        List<Sprite> sprites = new List<Sprite>();

        for (int i = 0; i < numberOfSprites; i++)
        {
            // Set the x position of the sprite
            int x = i * spriteWidth;

            // Create a new sprite from the sprite sheet
            sprites.Add(Sprite.Create(tex, new Rect(x, 0, spriteWidth, tex.height), new Vector2(0.5f, 0.5f)));
        }

        if (sprites != null && sprites.Count > 0)
        {
            UnifiedPonyObject upo = new UnifiedPonyObject();
            upo.SetUp(sprites, ponyGrid, ponyGridElmentPrefab, animatePonyGridElment);
            return upo;
        }
        else
        {
            return null;
        }
    }

    public void MakePonyFromGif(byte[] data, bool animatePonyGridElment = true)
    {
        MemoryStream memoryStream = new MemoryStream(data);
        BinaryReader binaryReader = new BinaryReader(memoryStream);
  
        GIFImage image = new GIFLoader().Load(binaryReader);
        Texture2D tex = new Texture2D(image.screen.width * image.imageData.Count, image.screen.height);
        var colors = tex.GetPixels32();
        for (int i = 0; i < image.imageData.Count; i++)
        {
            image.DrawImageTo(i, colors, tex.width, tex.height, image.screen.width * i, 0);
        }
        tex.SetPixels32(colors);
        tex.Apply();

        binaryReader.Close();

        MakePonyFromSprite(tex, image.imageData.Count, animatePonyGridElment);
    }

    public void SpawnPony(UnifiedPonyObject upo)
    {
        PonyController pony = MakeInstance(upo);
        pony.InitPush();
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
        pony.gameObject.SetActive(false);
        pony.collider.enabled = false;
        pony.inSpawningQueue = true;
        ponyQueue.Enqueue(pony);
    }

    public void ClearQueue()
    {
        ponyQueue.Clear();
    }
}

