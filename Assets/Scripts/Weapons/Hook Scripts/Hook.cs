using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    public enum HookState
    {
        rotating, // hook is in player control (rotating up and down)
        firedTowardsTarget, // the hook is moving towards a target
        firedGoingBack, // the hook is moving back to hookPositionBeforeShooting
    }

    public HookState hookState; // current hook state
    private Vector3 hookPositionBeforeShooting; // the hook's position before being fired
    private Vector3 nextHookPosition;
    private Collectable HookedCollectable;
    private bool isHoldingShield = false;

    // Anchors
    public Transform collectableAnchor;

    // shooting related vars
    public float shootingSpeed;
    public float maxShootingDistance; // how far will the hook go during shooting
    private float lastFrameDistance;
    private float currentFrameDistance;

    // rope renderer component
    private RopeRenderer ropeRenderer;


    void Awake()
    {
        ropeRenderer = GetComponent<RopeRenderer>();
    }

    void Start()
    {
        // init variables
        hookPositionBeforeShooting = transform.position;
        hookState = HookState.rotating;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "PickUp" && !HookedCollectable && (hookState == HookState.firedTowardsTarget || hookState == HookState.firedGoingBack))
        {
            collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            HookedCollectable = collision.gameObject.GetComponent<Collectable>();

            //aviv changes- can delete if works
            HookedCollectable.TowerCaught = transform.parent.parent.name;

            // TODO: rotate the pickup relatively to the hook
            //Vector3 rotation = new Vector3(0,0,0);
            //manaCollectable.transform.Rotate(rotation,Space.Self);

            HookedCollectable.fallingSpeed = 0;
            HookedCollectable.hookAnchorAttached = collectableAnchor;
            hookState = HookState.firedGoingBack;
            FindObjectOfType<AudioManager>().Play("Hook Grab");
        }


    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            hookState = HookState.firedGoingBack;
        }
    }

    // Update is called once per frame
    void Update()
    {
        HookShooting(); // managing hook shooting
        RenderRope(); // rendering the hook's rope 
    }

    void HookShooting()
    {
        nextHookPosition = transform.position;
        currentFrameDistance = Vector3.Distance(nextHookPosition, hookPositionBeforeShooting);

        if (hookState == HookState.firedTowardsTarget)
        {
            nextHookPosition -= transform.up * shootingSpeed * Time.deltaTime;

            if (currentFrameDistance > maxShootingDistance)
            {
                hookState = HookState.firedGoingBack;
            }
        }

        else if (hookState == HookState.firedGoingBack)
        {
            nextHookPosition += transform.up * shootingSpeed * Time.deltaTime;
            if (currentFrameDistance < 5f && currentFrameDistance > lastFrameDistance)
            {
                // hook came back to it's starting position
                OnHookCameBack();
            }
        }

        transform.position = nextHookPosition;
        lastFrameDistance = currentFrameDistance;
    }

    void RenderRope()
    {
        bool enableRenderer = hookState == HookState.firedTowardsTarget || hookState == HookState.firedGoingBack;
        ropeRenderer.renderLine(transform.position, enableRenderer);
    }

    public void Shoot()
    {
        GetComponent<Aiming>().enabled = false; // disable aiming while the hook is being used
        hookPositionBeforeShooting = transform.position;
        hookState = HookState.firedTowardsTarget;
    }

    public void OnHookCameBack()
    {
        nextHookPosition = hookPositionBeforeShooting;
        hookState = HookState.rotating;
        GetComponent<Aiming>().enabled = true; // enable aiming
        if (HookedCollectable)
        {
            if (HookedCollectable.gameObject.GetComponent<ManaCollectable>() != null) // collectable is mana type
            {
                GetComponentInParent<Tower>().OnCollectableCatch(HookedCollectable.gameObject);
                HookedCollectable = null;
                FindObjectOfType<AudioManager>().Play("Hook Tower");
            }
            else // collectable is shield type
            {
                if (isHoldingShield)
                {
                    // this shield collectable already came back to base
                    Destroy(HookedCollectable.gameObject);
                    FindObjectOfType<AudioManager>().Play("Hook Tower");
                    HookedCollectable = null;
                    isHoldingShield = false;
                }
                else
                {
                    isHoldingShield = true;
                    HookedCollectable.gameObject.GetComponent<ShieldCollectable>().switchToShield();
                }
            }
        }
        else // no collectable hooked
        {
            isHoldingShield = false;
        }
    }
}
