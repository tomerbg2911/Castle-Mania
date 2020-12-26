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
    private GameObject hookedCollectableGameObject = null;
    
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
        if (collision.gameObject.tag == "PickUp" && hookState == HookState.firedTowardsTarget)
        {
            collision.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            hookedCollectableGameObject = collision.gameObject;
            ManaCollectable manaCollectable = hookedCollectableGameObject.GetComponent<ManaCollectable>();

            // TODO: rotate the pickup relatively to the hook
            //Vector3 rotation = new Vector3(0,0,0);
            //manaCollectable.transform.Rotate(rotation,Space.Self);

            manaCollectable.fallingSpeed = 0;
            manaCollectable.hookAttached = collectableAnchor;

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
        Vector3 tempPosition = transform.position;
        currentFrameDistance = Vector3.Distance(tempPosition, hookPositionBeforeShooting);

        if (hookState == HookState.firedTowardsTarget)
        { 
            tempPosition -= transform.up * shootingSpeed * Time.deltaTime;

            if (currentFrameDistance > maxShootingDistance)
            {
                hookState = HookState.firedGoingBack;
            }
        }

        else if (hookState == HookState.firedGoingBack)
        { 
            tempPosition += transform.up * shootingSpeed * Time.deltaTime;
            if (currentFrameDistance < 5f && currentFrameDistance > lastFrameDistance)
            {
                // hook came back to it's starting position
                tempPosition = hookPositionBeforeShooting;
                hookState = HookState.rotating;
                GetComponent<Aiming>().enabled = true; // enable aiming
                if(hookedCollectableGameObject != null)
                {
                    GetComponentInParent<Tower>().OnCollectableCatch(hookedCollectableGameObject);
                    hookedCollectableGameObject = null;
                }
            }
        }

        transform.position = tempPosition;
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
}
