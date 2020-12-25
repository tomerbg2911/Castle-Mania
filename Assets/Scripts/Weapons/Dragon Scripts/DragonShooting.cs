using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonShooting : MonoBehaviour
{
    private Animator animator;
    public GameObject dragonFirePrefab;
    public bool isShooting;

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

    public void Shoot(Vector3 target)
    {
        isShooting = true;
        animator.SetTrigger("Shoot");
    }
}
