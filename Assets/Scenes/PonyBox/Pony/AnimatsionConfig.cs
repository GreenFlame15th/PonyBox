using B83.Image.GIF;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AnimatsionConfig : MonoBehaviour
{
    public AnimatsionGuide guide;
    public UnifiedPonyObject upo;
    public StaticAnimatorImage previewImage;
    public Texture2D tex;

    public AnimatsionConfig Invoke(byte[] fileData, bool isAnimatedGif)
    {
        guide = new AnimatsionGuide(fileData, isAnimatedGif);
        upo = new UnifiedPonyObject();

        if (guide.isAnimatedGif)
            try
            {
                guide.numberOfFrames = MakePonyFromGif();
            }
            catch
            {
                MakePonyFromPng();
            }
        else
            MakePonyFromPng();

        MakePonyFromSprite();

        previewImage.StartAnimetsion(upo);
        return this;
    }

    public AnimatsionConfig ReMakeSprite()
    {
        if(guide.isAnimatedGif)
            try
            {
                MakePonyFromGif();
            }
            catch
            {
                MakePonyFromPng();
            }
        else
            MakePonyFromPng();
        return ReAnimiateSprite();
    }

    public AnimatsionConfig ReAnimiateSprite()
    {
        MakePonyFromSprite();
        previewImage.upo = upo;
        return this;
    }

    public void Finish()
    {
        upo.ReadyToGo();
        GameObject.Destroy(this);
    }

    public void Discard()
    {
        PonyBoxManager.instance.areYouSurePopUp.Invoke("Discard changes?", () => GameObject.Destroy(this));
    }

    public void MakePonyFromPng()
    {
        Texture2D
        tex = new Texture2D(2, 2);
        tex.LoadImage(guide.fileData); //..this will auto-resize the texture dimensions.
                                 //formant to prevent bluer
        this.tex = tex;

    }

    public UnifiedPonyObject MakePonyFromSprite()
    {
        tex.filterMode = FilterMode.Point;

        int spriteWidth = tex.width / guide.numberOfFrames;

        List<Sprite> sprites = new List<Sprite>();

        for (int i = 0; i < guide.numberOfFrames; i++)
        {
            // Set the x position of the sprite
            int x = i * spriteWidth;

            // Create a new sprite from the sprite sheet
            sprites.Add(Sprite.Create(tex, new Rect(x, 0, spriteWidth, tex.height), new Vector2(0.5f, 0.5f)));
        }

        if (sprites != null && sprites.Count > 0)
        {
            upo.ReciveSprites(sprites);
            return upo;
        }
        else
        {
            return null;
        }
    }

    public int MakePonyFromGif()
    {
        MemoryStream memoryStream = new MemoryStream(guide.fileData);
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

        this.tex = tex;
        return (image.imageData.Count);
    }
}

[System.Serializable]
public class AnimatsionGuide
{
    public byte[] fileData;
    public bool isAnimatedGif;
    public int numberOfFrames;

    public AnimatsionGuide(byte[] fileData, bool isAnimatedGif)
    {
        this.fileData = fileData;
        this.isAnimatedGif = isAnimatedGif;
        this.numberOfFrames = 16;
    }
}
