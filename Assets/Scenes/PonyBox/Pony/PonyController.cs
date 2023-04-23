using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class PonyController : Queueable, IPointerDownHandler, IBeginDragHandler, IEndDragHandler
{
    public Rigidbody2D rigidBody;
    public UnifiedPonyObject upo;
    public Collider2D collider;

    public bool inQueue = false;
    public override void Fold()
    {
        inQueue = true;
        base.Fold();
    }

    public override void DeQueue()
    {
        inQueue = false;
    }

    private void OnEnable()
    {
        if (upo != null)
            StartAnimatsion();


        collider.enabled = true;
    }

    private void OnDisable()
    {
        collider.enabled = false;
    }

    #region Animator
    public SpriteRenderer spriteRenderer;
    
    public void SetUp(UnifiedPonyObject upo)
    {
        this.upo = upo;
    }

    public void StartAnimatsion()
    {
        StartCoroutine(Animator());
    }

    private int currentFrame = 0;
    private IEnumerator Animator()
    {
        while (true)
        {
            spriteRenderer.sprite = upo.sprites[currentFrame];
            currentFrame++;
            if (currentFrame >= upo.numberOfSprites)
            {
                currentFrame = 0;
            }
            yield return new WaitForSeconds(upo.scriptable.delay / (rigidBody.velocity.magnitude + 0.1f));
        }
    }
    #endregion
    #region addFroce
    public void OnPointerDown(PointerEventData eventData)
    {
        if(PonyBoxManager.instance.ponyClickMode == ClickMode.Push)
            addRanomeForceOnClick();
    }

    public void addRanomeForceOnClick()
    {
        if (rigidBody.velocity.x != 0 && rigidBody.velocity.y != 0)
        {
            addRanomeForce();
        }
        else
        {
            AddForwardForce();
        }

    }

    public void AddForwardForce()
    {
        if(rigidBody.velocity.x > 0 || rigidBody.velocity.y > 0)
        {
            rigidBody.AddForce(rigidBody.velocity.normalized * upo.scriptable.backtoScreenForce);
        }
        else
        {
            addRanomeForce();
        }
    }

    public void addRanomeForce()
    {
        Vector2 randomVector = UnityEngine.Random.insideUnitCircle;
        randomVector.Normalize();

        rigidBody.velocity.Set(0, 0);
        rigidBody.AddForce(randomVector * upo.scriptable.force);
    }
    public void InitPush()
    {
        Vector3 spawnPoint;
        if (Random.Range(0, 2) > 0.5)
        { spawnPoint = new Vector3(1.1f, 1.1f, 0); }
        else { spawnPoint = new Vector3(-0.1f, -0.1f, 0); }
        if (Random.Range(0, 2) > 0.5)
        { spawnPoint.x = Random.Range(-0.1f, 1.1f); }
        else { spawnPoint.y = Random.Range(-0.1f, 1.1f); }
        spawnPoint = PonyBoxManager.instance.mainCamer.ViewportToWorldPoint(spawnPoint);
        spawnPoint.z = 0;

        this.transform.position = spawnPoint;

        rigidBody.velocity = Vector2.zero;
        AddScreenCenterForce(1f);
    }

    public void AddScreenCenterForce(float detlataTime)
    {
        rigidBody.AddForce(getScreenCenterDirecion() * upo.scriptable.backtoScreenForce * detlataTime);
    }
    #endregion
    private Vector2 lastBounce;
    private bool lastBounceVertical;
    private bool bounceConsidon() { return PonyBoxManager.instance.borderMode == BorderMode.HARD; }

    public void FixedUpdate()
    {
        hasScreenSenterDrecion = false;

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, 0), upo.scriptable.selfRightingSpeed * Time.deltaTime);

        //bounce and off screen detecion
        if(PonyBoxManager.instance.borderMode != BorderMode.NON)
        {
            if (transform.position.x > PonyBoxManager.instance.screenBounds.x - spriteRenderer.bounds.extents.x)
            {
                if (bounceConsidon() && rigidBody.velocity.x > 0)
                {
                    rigidBody.velocity = new Vector2(-rigidBody.velocity.x, rigidBody.velocity.y);
                    onBounce(false);
                }
                else
                    AddScreenCenterForce(Time.fixedDeltaTime);
            }
            else if (transform.position.x < -PonyBoxManager.instance.screenBounds.x + spriteRenderer.bounds.extents.x)
            {
                if (bounceConsidon() && rigidBody.velocity.x < 0)
                {
                    rigidBody.velocity = new Vector2(-rigidBody.velocity.x, rigidBody.velocity.y);
                    onBounce(false);
                }
                else
                    AddScreenCenterForce(Time.fixedDeltaTime);
            }
            if (transform.position.y > PonyBoxManager.instance.screenBounds.y - spriteRenderer.bounds.extents.y)
            {
                if (bounceConsidon() && rigidBody.velocity.y > 0)
                {
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, -rigidBody.velocity.y);
                    onBounce(true);
                }
                else
                    AddScreenCenterForce(Time.fixedDeltaTime);
            }
            else if(transform.position.y < -PonyBoxManager.instance.screenBounds.y + spriteRenderer.bounds.extents.y)
            {
                if (bounceConsidon() && rigidBody.velocity.y < 0)
                {
                    rigidBody.velocity = new Vector2(rigidBody.velocity.x, -rigidBody.velocity.y);
                    onBounce(true);
                }
                else
                    AddScreenCenterForce(Time.fixedDeltaTime);
            }
        }

        //sugar rush mode
        if (PonyBoxManager.instance.sugarRush && !beginDraged && rigidBody.velocity.sqrMagnitude < upo.scriptable.speedLimit)
        {
            AddForwardForce();
        }

        //flip in not upside down
        if (transform.rotation.z < 0.5 && transform.rotation.z > -0.5)
            spriteRenderer.flipX = rigidBody.velocity.x > 0;

        //wirlpool
        if(PonyBoxManager.instance.whirlpool)
        {
            rigidBody.AddForce(getScreenCenterDirecion() * Time.fixedDeltaTime * upo.scriptable.whirlpoolPullForce);
            rigidBody.AddForce(getScreenCenterDirecion().Rotate90() * Time.fixedDeltaTime * upo.scriptable.whirlpoolSpinForce);
        }

        //drag handler
        if(beginDraged)
        {
            Vector3 direcion = PonyBoxManager.instance.mainCamer.ScreenToWorldPoint(Input.mousePosition);
            direcion.z = 0;
            direcion -= transform.position;

            if (oppositeFloat(direcion.x, rigidBody.velocity.x))
                    direcion.x *= upo.scriptable.pullInForceMultiplayer;

            if (oppositeFloat(direcion.y, rigidBody.velocity.y))
                direcion.y *= upo.scriptable.pullInForceMultiplayer;

            rigidBody.AddForce(direcion * upo.scriptable.dragForce * Time.fixedDeltaTime);
        }

        ignoreNextBounceThing = false;

    }

    private bool oppositeFloat(float a, float b)
    {
        return (a < 0) == (b > 0);
    }

    private void onBounce(bool thisBounceVertical)
    {

        if (thisBounceVertical != lastBounceVertical &&
            !beginDraged &&
            !ignoreNextBounceThing &&
            ((Vector2)transform.position - lastBounce).sqrMagnitude < upo.scriptable.bouceLinency * PonyBoxManager.instance.mainCamer.orthographicSize)
        {
            PonyBoxManager.instance.incrmentBouncCounter();
        }

        lastBounce = transform.position;
        lastBounceVertical = thisBounceVertical;
    }

    private Vector3 screenSenterDrecion;
    private bool hasScreenSenterDrecion;
    public Vector2 getScreenCenterDirecion()
    {
        if(!hasScreenSenterDrecion)
        {
            Vector2 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2);
            Vector2 worldCenter = Camera.main.ScreenToWorldPoint(screenCenter);
            screenSenterDrecion =(worldCenter - (Vector2)transform.position).normalized;
            hasScreenSenterDrecion = true;
        }
        return screenSenterDrecion;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.TryGetComponent<PonyController>(out PonyController ponyController))
        {
            if(PonyBoxManager.instance.hearts && ponyController.gameObject.GetInstanceID() > gameObject.GetInstanceID() && collision.relativeVelocity.sqrMagnitude > upo.scriptable.minCollisonVelocty)
            {
                HearScript heartScript = PonyBoxManager.instance.heartQueue.GetGameObject(collision.contacts[0].point).GetComponent<HearScript>();
                Color colorHT = UnityEngine.Random.ColorHSV(0,1,1,1,0.7f,1);
                heartScript.StartScript(colorHT, collision.relativeVelocity.magnitude);
            }
        }
    }
    private bool beginDraged;
    private bool ignoreNextBounceThing;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if(PonyBoxManager.instance.ponyClickMode == ClickMode.Drag)
        {
            beginDraged = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        beginDraged = false;
        ignoreNextBounceThing = true;
    }
}