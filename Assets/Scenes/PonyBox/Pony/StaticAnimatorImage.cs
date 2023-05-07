using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaticAnimatorImage : MonoBehaviour
{
    public UnifiedPonyObject upo = null;
    private int currentFrame = 0;
    public Image image;

    public void StartAnimetsion(UnifiedPonyObject upo)
    {
        this.upo = upo;
        StartCoroutine(Animator());

    }

    private void OnEnable()
    {
        if(upo != null)
            StartCoroutine(Animator());
    }

    private IEnumerator Animator()
    {

        while (true)
        {
            if(upo != null)
            {
                currentFrame++;
                if (currentFrame >= upo.numberOfSprites)
                {
                    currentFrame = 0;
                }
                image.sprite = upo.sprites[currentFrame];
                yield return new WaitForSeconds(upo.scriptable.staticDelay);
            }
            else
                yield return new WaitForSeconds(0.1f);
            
        }
    }

}
