using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonShooting : MonoBehaviour
{
    private Animator animator;
    public GameObject dragonFirePrefab;
    public bool isShooting;
    public Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        isShooting = false;
        animator = GetComponent<Animator>();
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

    public void spawnFire()
    {
        GameObject newFire = Instantiate(dragonFirePrefab, transform.position, transform.rotation) as GameObject;
        newFire.GetComponent<DragonFire>().fireTarget = target;
        GetComponent<DragonAiming>().destroyAllTargets();
    }

    public void OnAnimationFinished()
    {
        isShooting = false;
        animator.SetBool("IsShooting", false);
    }
}
