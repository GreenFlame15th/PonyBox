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

    public void StartAnimetsion(UnifiedPonyObject upo, bool animate = true)
    {
        this.upo = upo;
        if(gameObject.activeSelf && animate)
        {
            StartCoroutine(Animator());
        }
        
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
            image.sprite = upo.sprites[currentFrame];
            currentFrame++;
            if (currentFrame >= upo.numberOfSprites)
            {
                currentFrame = 0;
            }
            yield return new WaitForSeconds(upo.scriptable.staticDelay);
        }
    }

    internal void StartAnimetsion()
    {
        throw new NotImplementedException();
    }
}
