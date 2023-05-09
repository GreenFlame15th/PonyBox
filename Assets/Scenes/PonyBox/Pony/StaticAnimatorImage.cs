using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StaticAnimatorImage : MonoBehaviour
{
    public UnifiedPonyObject upo = null;
    private int currentFrame = 0;
    public Image image;

    private void OnEnable()
    {
        StartCoroutine(Animator());
    }

    private IEnumerator Animator()
    {

        while (true)
        {
            if(upo != null && upo.sprites != null && upo.sprites.Length > 0)
            {
                currentFrame++;
                if (currentFrame >= upo.numberOfSprites)
                {
                    currentFrame = 0;
                }
                if(upo.sprites[currentFrame] != null)
                    image.sprite = upo.sprites[currentFrame];
                yield return new WaitForSeconds(upo.scriptable.staticDelay);
            }
            else
                yield return new WaitForSeconds(0.1f);
            
        }
    }

}
