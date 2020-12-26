using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAiming : MonoBehaviour
{
    public bool isAiming = true;
    public int minAmountOfMana = 3;

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
    private GameObject finalTarget;


    // Start is called before the first frame update
    void Start()
    {
        // init vars
        shoot = GetComponentInParent<Tower>().shoot;  // shoot keyboard key
        dragonTargets = new DragonTarget[numOfTargets];  // dragon targets array 
        countTargets = 0; // current number of targets
        isAiming = false; // dragon is now aiming targets
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void StartAimingSequence()
    {
        GetComponentInParent<Tower>().manaAmount -= minAmountOfMana;  // reduce tower's mana Amount
        isAiming = true;
    }

    void EndAimingSequence()
    {
        isAiming = false;
        Vector3 combinedTarget = CombineTargets(); // combine all targets
        finalTarget = Instantiate(finalTargetPrefab, combinedTarget, Quaternion.identity, targetsRect) as GameObject;
        GetComponent<DragonShooting>().target = combinedTarget;
        GetComponent<DragonShooting>().Shoot();
        GetComponentInParent<Tower>().SwitchToNextWeapon(); // switch tower's weapon
    }

    void GetInput()
    {
        if (Input.GetKeyDown(shoot))
        {
            if (countTargets == 0)
            {
                StartAimingSequence();
            }

            if(isAiming)
            {
                if(countTargets > 0 && dragonTargets[countTargets - 1].enabled
                ) // disable previous target movement (if necessary)
                {
                    dragonTargets[countTargets - 1].enabled = false;
                }

                if(countTargets < numOfTargets) // instantiate a new target
                {
                    InstantiateTarget();
                }
                else // no more targets to instantiate 
                {
                    EndAimingSequence();
                }
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
        foreach (DragonTarget dragonTarget in dragonTargets)
        {
            dragonTargetsSum += dragonTarget.transform.position;
        }
        return dragonTargetsSum / numOfTargets;
    }

    public void destroyAllTargets()
    {
        foreach (DragonTarget dragonTarget in dragonTargets)
        {
            Destroy(dragonTarget.gameObject);
        }
        Destroy(finalTarget);
        countTargets = 0;
        //isAiming = true; 
    }
}
