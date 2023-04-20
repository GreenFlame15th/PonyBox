using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HearScript : Queueable
{
    private Vector2 orgin;
    private Vector2 direcion;
    public HeartScriptable scriptable;
    private float lifeSpam;
    public SpriteRenderer spriteRenderer;
    private Color color;
    public SpriteRenderer shineRenderer;
    private Color shinecolor = Color.white;
    private float distance;

    public void StartScript(Color newColor, float scale)
    { 
        scale = (scale + scriptable.minhartSize) * scriptable.scaleMiltiplaier;
        distance = scriptable.animationDistance * scale;
        transform.localScale = Vector3.one * scale;
        spriteRenderer.color = newColor;
        lifeSpam = 0;
        direcion = UnityEngine.Random.insideUnitCircle;
        direcion.Normalize();

        orgin = this.transform.position;

        color = spriteRenderer.color;

    }
    private void Update()
    {
        lifeSpam += Time.deltaTime;
        
        if(lifeSpam > scriptable.animationTime)
        {
            this.Fold();
        }
        else
        {
            float point = lifeSpam / scriptable.animationTime;

            this.transform.position = orgin + (direcion * distance * scriptable.travelCurve.Evaluate(point));

            color.a = scriptable.fade.Evaluate(point);
            spriteRenderer.color = color;

            shinecolor.a = color.a;
            shineRenderer.color = shinecolor;
        }
    }
}
