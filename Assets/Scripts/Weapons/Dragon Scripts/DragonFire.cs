using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFire : MonoBehaviour
{
    public Vector3 fireTarget;
    public float speed = 5.0f;
    public GameObject explosionPrefab;

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
        }

        if (collision.gameObject.CompareTag("PickUp") || collision.gameObject.CompareTag("Hook"))                         // not working needs to work on.
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(this.GetComponent<PolygonCollider2D>(), collision.GetComponent<Collider2D>());
        }

        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionPrefab, collision.transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, 3);

    }

}
