using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookMovement : MonoBehaviour
{
    enum HookState
    {
        idle, // hook is not in player control
        rotating, // hook is in player control (rotating up and down)
        firedTowardsTarget, // the hook is moving towards a target
        firedGoingBack, // the hook is moving back to HookPositionBeforeShooting
    }

    private HookState hookState; // current hook state
    private Vector3 HookPositionBeforeShooting; // the hook's position before being fired
    private GameObject hookedCollectableGameObject = null;
    
    // Anchors
    public Transform collectableAnchor;
    public Transform rotationAnchor;

    // rotation related vars
    public float rotationSpeed = 10f;
    public float minRotationZ = 0f;
    public float maxRotationZ = 360f;

    // shooting related vars
    public float shootingSpeed;
    public float maxShootingDistance; // how far will the hook go during shooting
    private float lastFrameDistance;
    private float currentFrameDistance;

    // rope renderer component
    private RopeRenderer ropeRenderer;

    // keyboard keys
    private KeyCode up;
    private KeyCode down;
    private KeyCode shoot;
    private KeyCode switchWeapon;

    void Awake()
    {
        ropeRenderer = GetComponent<RopeRenderer>();
    }

    void Start()
    {
        // init variables
        HookPositionBeforeShooting = transform.position;
        hookState = HookState.rotating;

        // keyboard keys init
        up = GetComponentInParent<Tower>().up;
        down = GetComponentInParent<Tower>().down;
        shoot = GetComponentInParent<Tower>().shoot;
        switchWeapon = GetComponentInParent<Tower>().switchWeapon;
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
        GetInput(); // get keyboard inputs
        HookShooting(); // managing hook shooting
        RenderRope(); // rendering the hook's rope 
    }

    void Rotate(Vector3 axis)
    {
        if(hookState == HookState.rotating)
        {
            transform.RotateAround(rotationAnchor.position, axis, rotationSpeed * Time.deltaTime);
        }
    }

    void HookShooting()
    {
        Vector3 tempPosition = transform.position;
        currentFrameDistance = Vector3.Distance(tempPosition, HookPositionBeforeShooting);

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
                tempPosition = HookPositionBeforeShooting;
                hookState = HookState.rotating;
                Destroy(hookedCollectableGameObject); // TODO: add animation
            }
        }

        transform.position = tempPosition;
        lastFrameDistance = currentFrameDistance;
    }

    void GetInput()
    {
        if (Input.GetKey(down) && transform.eulerAngles.z > minRotationZ)
        {
            print("***" + transform.eulerAngles.z);
            Rotate(-transform.forward);
        }
        if (Input.GetKey(up) && transform.eulerAngles.z < maxRotationZ)
        {
            Rotate(transform.forward);
        }
        if(Input.GetKeyDown(shoot) && hookState == HookState.rotating)
        {
            HookPositionBeforeShooting = transform.position;
            hookState = HookState.firedTowardsTarget;
        }
    }

    void RenderRope()
    {
        bool enableRenderer = hookState == HookState.firedTowardsTarget || hookState == HookState.firedGoingBack;
        ropeRenderer.renderLine(transform.position, enableRenderer);
    }
}
