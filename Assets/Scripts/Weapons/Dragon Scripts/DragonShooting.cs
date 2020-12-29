using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonShooting : MonoBehaviour
{
    private Animator animator;
    public GameObject dragonFirePrefab;
    public bool isShooting;
    public Vector3 target;
    private Tower parentTowerScript;

    // Start is called before the first frame update
    void Start()
    {
        isShooting = false;
        animator = GetComponent<Animator>();
        parentTowerScript = GetComponentInParent<Tower>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot()
    {
        isShooting = true;
        animator.SetBool("IsShooting", true);
    }

    public IEnumerator spawnFire()
    {

        GetComponent<DragonAiming>().destroyAllTargets();
        for (int i = 1; i <= parentTowerScript.manaAmount; i++)
        {
            if (i == 4) //shooting max 3 balls of fire 
            {
                break;
            }
            GameObject newFire = Instantiate(dragonFirePrefab, transform.position, transform.rotation) as GameObject;
            newFire.GetComponent<DragonFire>().fireTarget = target;
            yield return new WaitForSeconds(0.1f);
        }

        parentTowerScript.setManaAmount(0);
    }

    public void OnAnimationFinished()
    {
        isShooting = false;
        animator.SetBool("IsShooting", false);
    }
}
