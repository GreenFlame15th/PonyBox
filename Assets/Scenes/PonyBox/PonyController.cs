using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PonyController : MonoBehaviour, IPointerDownHandler
{
    public PonyScriptable scriptable;
    public Rigidbody2D rigidBody;

    #region Animator
    public SpriteRenderer spriteRenderer;
    private int numberOfSprites;
    private List<Sprite> sprites = new List<Sprite>();
    public void StartAnimetsion(Texture2D spriteSheet, int numberOfSprites)
    {
        this.numberOfSprites = numberOfSprites;

        int spriteWidth = spriteSheet.width / numberOfSprites;


        for (int i = 0; i < numberOfSprites; i++)
        {
            // Set the x position of the sprite
            int x = i * spriteWidth;

            // Create a new sprite from the sprite sheet
            sprites.Add(Sprite.Create(spriteSheet, new Rect(x, 0, spriteWidth, spriteSheet.height), new Vector2(0.5f, 0.5f)));
        }

        StartCoroutine(Animator());
        AddScreenCenterForc(10f);
    }

    private int currentFrame = 0;
    private IEnumerator Animator()
    {
        while (true)
        {
            spriteRenderer.sprite = sprites[currentFrame];
            currentFrame++;
            if (currentFrame >= numberOfSprites)
            {
                currentFrame = 0;
            }
            yield return new WaitForSeconds(scriptable.delay / (rigidBody.velocity.magnitude + 0.1f));
        }
    }
    #endregion
    #region Bouncer

    #endregion
    public void OnPointerDown(PointerEventData eventData)
    {
        addRanomeForceOnClick();
    }

    public void addRanomeForceOnClick()
    {
        if(rigidBody.velocity.sqrMagnitude < scriptable.speedLimit)
        {
            addRanomeForce(); ;
        }
        else
        {
            rigidBody.AddForce(rigidBody.velocity.normalized * scriptable.backtoScreenForce);
        }

    }

    public void addRanomeForce()
    {
        Vector2 randomVector = UnityEngine.Random.insideUnitCircle;
        randomVector.Normalize();

        rigidBody.velocity.Set(0, 0);
        rigidBody.AddForce(randomVector * scriptable.force);
    }

    private Vector2 lastBounce;
    private bool lastBounceVertical;

    public void FixedUpdate()
    {

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), scriptable.selfRightingSpeed * Time.deltaTime);

        //bounce and off screen detecion
        if (transform.position.x > PonyBoxManager.instance.screenBounds.x - spriteRenderer.bounds.extents.x)
        {
            if (rigidBody.velocity.x > 0)
            {
                rigidBody.velocity = new Vector2(-rigidBody.velocity.x, rigidBody.velocity.y);
                onBounce(false);
            }

            AddScreenCenterForc(Time.fixedDeltaTime);
        }
        if (transform.position.x < -PonyBoxManager.instance.screenBounds.x + spriteRenderer.bounds.extents.x)
        {
            if(rigidBody.velocity.x < 0)
            {
                rigidBody.velocity = new Vector2(-rigidBody.velocity.x, rigidBody.velocity.y);
                onBounce(false);
            }

            AddScreenCenterForc(Time.fixedDeltaTime);
        }
        if (transform.position.y > PonyBoxManager.instance.screenBounds.y - spriteRenderer.bounds.extents.y)
        {
            if(rigidBody.velocity.y > 0)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, -rigidBody.velocity.y);
                onBounce(true);
            }

            AddScreenCenterForc(Time.fixedDeltaTime);
        }
        if (transform.position.y < -PonyBoxManager.instance.screenBounds.y + spriteRenderer.bounds.extents.y)
        {
            if(rigidBody.velocity.y < 0)
            {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, -rigidBody.velocity.y);
                onBounce(true);
            }

            AddScreenCenterForc(Time.fixedDeltaTime);
        }

        if (PonyBoxManager.instance.sugarRush && rigidBody.velocity.sqrMagnitude < scriptable.speedLimit)
        {
            rigidBody.velocity = Vector2.zero;
            addRanomeForce();
        }

        if(transform.rotation.z < 0.5 && transform.rotation.z > -0.5)
            spriteRenderer.flipX = rigidBody.velocity.x > 0;
    }

    private void onBounce(bool thisBounceVertical)
    {

        if (thisBounceVertical != lastBounceVertical &&
            ((Vector2)transform.position - lastBounce).sqrMagnitude < scriptable.bouceLinency * PonyBoxManager.instance.mainCamer.orthographicSize)
        {
            PonyBoxManager.instance.incrmentBouncCounter();
        }

        lastBounce = transform.position;
        lastBounceVertical = thisBounceVertical;
    }

    public void AddScreenCenterForc(float detlataTime)
    {
        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        Vector3 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);
        Vector2 direction = (worldCenter - transform.position).normalized;
        rigidBody.AddForce(direction* scriptable.backtoScreenForce * detlataTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<PonyController>(out PonyController ponyController))
        {
            if(PonyBoxManager.instance.hearts && ponyController.gameObject.GetInstanceID() > gameObject.GetInstanceID() && collision.relativeVelocity.sqrMagnitude > scriptable.minCollisonVelocty)
            {
                HearScript heartScript = PonyBoxManager.instance.heartQueue.GetGameObject(collision.contacts[0].point).GetComponent<HearScript>();
                Color colorHT = UnityEngine.Random.ColorHSV(0,1,1,1,0.7f,1);
                heartScript.StartScript(colorHT, collision.relativeVelocity.magnitude);
            }
        }
    }
}
