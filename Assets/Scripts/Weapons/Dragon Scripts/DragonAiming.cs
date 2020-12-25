﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAiming : MonoBehaviour
{
    private bool isAiming = true;

    // keyboard keys
    private KeyCode shoot;

    // game objects
    public GameObject targetPrefab;
    public GameObject finalTargetPrefab;
    public Transform targetsRect;

    // targets
    public int numOfTargets = 3;
    private int countTargets = 0;
    private DragonTarget[] dragonTargets;


    // Start is called before the first frame update
    void Start()
    {
        // init vars
        shoot = GetComponentInParent<Tower>().shoot;  // shoot keyboard key
        dragonTargets = new DragonTarget[numOfTargets];  // dragon targets array 
        countTargets = 0; // current number of targets
        isAiming = true; // dragon is now aiming targets
    }

    // Update is called once per frame
    void Update()
    {
        if(isAiming)
        {
            GetInput();
        }
    }

    void GetInput()
    {
        if (Input.GetKeyDown(shoot))
        {
            if (countTargets > 0 && dragonTargets[countTargets - 1].enabled) // a target is currently moving
            {
                dragonTargets[countTargets - 1].enabled = false;
            }

            if(countTargets < numOfTargets)
            {
                InstantiateTarget();
            }
            else
            {
                // aiming is done, combine all targets and activate DragonShooting
                isAiming = false;
                Vector3 combinedTarget = CombineTargets();
                Instantiate(finalTargetPrefab, combinedTarget, Quaternion.identity, targetsRect);
                GetComponent<DragonShooting>().Shoot(combinedTarget);
            }
        }
    }

    void InstantiateTarget()
    {
        GameObject newTarget = Instantiate(targetPrefab, targetsRect.position, Quaternion.identity, targetsRect) as GameObject;
        dragonTargets[countTargets] = newTarget.GetComponent<DragonTarget>();
        countTargets++;
    }

    Vector3 CombineTargets()
    {
        Vector3 dragonTargetsSum = Vector3.zero;
        foreach(DragonTarget dragonTarget in dragonTargets)
        {
            dragonTargetsSum += dragonTarget.transform.position;
        }
        return dragonTargetsSum / numOfTargets;
    }
}
