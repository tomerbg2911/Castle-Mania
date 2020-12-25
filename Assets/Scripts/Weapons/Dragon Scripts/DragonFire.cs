using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFire : MonoBehaviour
{
    public Vector3 fireTarget;
    public float speed = 5.0f;

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();

    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, fireTarget, speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "gate")
        {
            Tower towerGotHit = collision.transform.GetComponentInParent<Tower>();
            towerGotHit.onDragonFireHitGate();
            Destroy(gameObject);
        }
    }
}
