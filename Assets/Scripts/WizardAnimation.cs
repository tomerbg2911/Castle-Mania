using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardAnimation : MonoBehaviour
{
    private Tower tower;
    private Animator animator;
    private DragonShooting dragonShooting;

    // Start is called before the first frame update
    void Start()
    {
        tower = GetComponentInParent<Tower>();
        animator = GetComponent<Animator>();
        dragonShooting = tower.GetComponentInChildren<DragonShooting>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetInteger("mana", tower.manaAmount);
        animator.SetBool("isShooting", dragonShooting.isShooting);
    }
}
