using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFire : MonoBehaviour
{
    public Vector3 fireTarget;
    public float speed = 5.0f;
    public float explosionDelay = 0.3f;
    public GameObject explosionPrefab;

    // Update is called once per frame
    void Update()
    {
        MoveTowardsTarget();

    }

    void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, fireTarget, speed * Time.deltaTime);
        if(Vector3.Distance(transform.position, fireTarget) <= 0.1f)
        {
            ExplodeAndDestroy(transform.position);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "gate")
        {
            Tower towerGotHit = collision.transform.GetComponentInParent<Tower>();
            towerGotHit.onDragonFireHitGate();
            //ExplodeAndDestroy(collision.transform.position);
        }

        //if (collision.gameObject.CompareTag("PickUp") || collision.gameObject.CompareTag("Hook"))                         // not working needs to work on.
        //{
        //    Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.GetComponent<Collider2D>());
        //    Physics2D.IgnoreCollision(this.GetComponent<PolygonCollider2D>(), collision.GetComponent<Collider2D>());
        //}

        //ExplodeAndDestroy(collision.transform.position);
    }

    void ExplodeAndDestroy(Vector3 positionForExplosion)
    {
        Destroy(gameObject);
        GameObject explosion = Instantiate(explosionPrefab, positionForExplosion, Quaternion.identity) as GameObject;
        StartCoroutine(explosionEnumerator(explosionDelay, positionForExplosion));
    }

    IEnumerator explosionEnumerator(float waitingTime, Vector3 positionForExplosion)
    {
        yield return new WaitForSeconds(waitingTime);
        GameObject explosion = Instantiate(explosionPrefab, positionForExplosion, Quaternion.identity) as GameObject;
        Destroy(explosion, 3);
    }

}
