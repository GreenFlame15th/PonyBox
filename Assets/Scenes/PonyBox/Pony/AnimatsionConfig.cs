using B83.Image.GIF;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AnimatsionConfig : MonoBehaviour
{
    public AnimatsionGuide guide;
    public UnifiedPonyObject upo;
    public StaticAnimatorImage previewImage;
    public Texture2D tex;

    public MiltySelectElement gifToggle;

    #region managments
    public AnimatsionConfig Invoke(byte[] fileData, bool isAnimatedGif)
    {
        guide = new AnimatsionGuide(fileData, isAnimatedGif);
        upo = new UnifiedPonyObject();

        return setUp(true);
    }

    public AnimatsionConfig setUp(bool overWriteFrames)
    {
        if (guide.isAnimatedGif)
            try
            {
                if (overWriteFrames)
                    guide.numberOfFrames = MakePonyFromGif();
                else
                    MakePonyFromGif();

                gifToggle.SoftToggle();
            }
            catch
            {
                MakePonyFromPng();
            }
        else
            MakePonyFromPng();

        ReAnimiateSprite();
        framesField.text = guide.numberOfFrames + "";
        previewImage.upo = upo;
        return this;
    }

    public AnimatsionConfig ReMakeSprite()
    {
        if (guide.isAnimatedGif)
            try
            {
                MakePonyFromGif();
                return ReAnimiateSprite();
            }
            catch (Exception e)
            {
                PonyBoxManager.instance.alarte.Invoke("Failed to read gif","Your sprite could not be animated as gif. Make sure it is valid gif");
#if UNITY_EDITOR
                Debug.LogError(e);
#endif
            }
        try
        {
            MakePonyFromPng();
            return ReAnimiateSprite();
        }
        catch (Exception e)
        {
            PonyBoxManager.instance.alarte.Invoke("Failed to readfile", "Your sprite could not be animated");
#if UNITY_EDITOR
            Debug.LogError(e);
#endif

        }
        return ReAnimiateSprite();
    }

    public AnimatsionConfig ReAnimiateSprite()
    {
        MakePonyFromSprite();
        guide.ySclaeForPrewView = (float)guide.numberOfFrames * tex.height / tex.width;
        guide.ySclaeForPrewView *= 0.9f;
        previewImage.upo = upo;
        reScale();
        return this;
    }
    public AnimatsionConfig ReadGuide(AnimatsionGuide guide, byte[] fileData)
    {
        this.guide = new AnimatsionGuide(guide, fileData);
        if(upo == null)
        {
            upo = new UnifiedPonyObject();
        }
        setUp(false);
        previewImage.upo = upo;
        return this;
    }
    public MiltySelectElement flipNo;
    public MiltySelectElement selfRightingNo;
    public bool isEditing = false;
    public AnimatsionConfig openEdit(UnifiedPonyObject upo)
    {
        this.upo = upo;
        guide = upo.guide;
        framesField.text = guide.numberOfFrames + "";
        previewImage.upo = upo;

        writeX();
        writeY();

        if(guide.isAnimatedGif)
            gifToggle.SoftToggle();

        if (!guide.selfRighting)
            selfRightingNo.SoftToggle();

        if (!guide.flip)
            flipNo.SoftToggle();

        ReMakeSprite();
        reScale();
        isEditing = true;

        return this;
    }

    public void Finish()
    {
        upo.guide = guide;
        MakePonyFromSprite();
        upo.ReadyToGo(!isEditing);
        PonyBoxManager.instance.animatsionConfigs.Remove(this);
        GameObject.Destroy(this.gameObject);
    }
    public void ApplyToAll()
    {
        AnimatsionGuide guideToApply = this.guide;
        PonyBoxManager.instance.animatsionConfigs.Remove(this);

        Debug.Log(PonyBoxManager.instance.animatsionConfigs.Count);

        AnimatsionConfig config;
        while (PonyBoxManager.instance.animatsionConfigs.Count > 0)
        {
            config = PonyBoxManager.instance.animatsionConfigs[0];
            config.ReadGuide(guideToApply, config.guide.fileData);
            config.Finish();
            Debug.Log(PonyBoxManager.instance.animatsionConfigs.Count);
        }

        Finish();
    }

    private void Awake()
    {
        PonyBoxManager.instance.animatsionConfigs.Add(this);
    }

    public void Discard()
    {

        PonyBoxManager.instance.areYouSurePopUp.Invoke("Discard changes?", () => {
            PonyBoxManager.instance.animatsionConfigs.Remove(this);
            Destroy(this.gameObject);
        
        });
    }

    public void MakePonyFromPng()
    {
        tex = new Texture2D(2, 2);
        tex.LoadImage(guide.fileData);
    }

    public UnifiedPonyObject MakePonyFromSprite()
    {
        Debug.Log("sprtieFormating");

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
            Debug.LogWarning("trying ot make pony with empty sprties");
            return null;
        }
    }
#endregion
    #region guideChanges
    public void changeType(bool gif)
    {
        guide.isAnimatedGif = gif;
        ReMakeSprite();
    }
    public InputField framesField;
    public void addFrames(int toAdd)
    {
        setFrames(guide.numberOfFrames + toAdd);
    }
    public void setFrames(int newFrames)
    {
        if(newFrames > 0)
        {
            guide.numberOfFrames = newFrames;
            framesField.text = newFrames + "";
            ReAnimiateSprite();
        }
        else
        {
            setFrames(1);
        }
    }
    public void setFrames()
    {
        setFrames(int.Parse(framesField.text));
    }
    public void setFlip(bool flip)
    {
        guide.flip = flip;
    }
    public void setSelfRighting(bool selfRighting)
    {
        guide.selfRighting = selfRighting;
    }

    #region scale
    public Slider sliderX;
    public Slider sliderY;
    public InputField fieldX;
    public InputField fieldY;
    public GraficToggle linkedToggle;
    public bool readToread = true;

    public void updateXraw(float value)
    {
        if(readToread)
        {
            readToread = false;

            if(linkedToggle.on)
            {
                guide.scaleY *= value / guide.scaleX;
                writeY();
            }
            guide.scaleX = value;
            writeX();
            reScale();
            readToread = true;
        }
    }
    public void updateYraw(float value)
    {
        if (readToread)
        {
            readToread = false;

            if (linkedToggle.on)
            {
                guide.scaleX *= value / guide.scaleY;
                writeX();
            }
            guide.scaleY = value;
            writeY();
            reScale();
            readToread = true;
        }
    }
    public void readXslider()
    {
        updateXraw(sliderMath(sliderX.value));
    }
    public void readYslider()
    {
        updateYraw(sliderMath(sliderY.value));
    }
    public void readXfiled()
    {
        updateXraw(float.Parse(fieldX.text));
    }
    public void readYfiled()
    {
        updateYraw(float.Parse(fieldY.text));
    }
    public void writeX()
    {
        sliderX.value = reversSliderMath(guide.scaleX);
        fieldX.text = guide.scaleX + "";
    }
    public void writeY()
    {
        sliderY.value = reversSliderMath(guide.scaleY);
        fieldY.text = guide.scaleY + "";
    }
    public void resetSclae()
    {
        guide.scaleX = 1f;
        guide.scaleY = 1f;
        writeX();
        writeY();
        reScale();
    }
    public float sliderMath(float value)
    {
        return (float)(Math.Pow(10, value) / 10);
    }
    public float reversSliderMath(float value)
    {
        return (float)Math.Log10(value * 10);
    }


    public void reScale()
    {
        previewImage.transform.localScale =
            new Vector3(guide.scaleX, guide.scaleY * guide.ySclaeForPrewView, 1);
    }
    #endregion
    #endregion

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
    public bool flip;
    public bool selfRighting;
    public float ySclaeForPrewView;
    public float scaleX = 1f;
    public float scaleY = 1f;

    public AnimatsionGuide(byte[] fileData, bool isAnimatedGif)
    {
        this.fileData = fileData;
        this.isAnimatedGif = isAnimatedGif;
        numberOfFrames = 16;
        flip = true;
        selfRighting = true;
    }
    public AnimatsionGuide(AnimatsionGuide orgin, byte[] fileData)
    {
        this.fileData = fileData;
        this.isAnimatedGif = orgin.isAnimatedGif;
        this.numberOfFrames = orgin.numberOfFrames;
        this.flip = orgin.flip;
        this.selfRighting = orgin.selfRighting;
        this.ySclaeForPrewView = orgin.ySclaeForPrewView;
        scaleX = orgin.scaleX;
        scaleY = orgin.scaleY;
    }
}
