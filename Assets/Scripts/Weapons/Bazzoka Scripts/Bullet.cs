using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private  GameObject target;
    public string fatherCannon;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        target = collision.collider.gameObject;
        if (target.CompareTag("Soldier"))
        {
            if (string.Equals(collision.collider.transform.parent.name, fatherCannon)) //if the bullet and the soldier we hit are from the same tower-ignore
            {
                //Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.collider);
                Destroy(gameObject);
            }
            else
            { 
                target.GetComponent<SoldierHealth>().changeHealth(-35);
                Destroy(gameObject, 0.1f);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            Destroy(gameObject, 0.1f);
        }

        if (collision.CompareTag("PickUp"))
        {
            collision.gameObject.GetComponent<ManaCollectable>().fallingSpeed *= 5;
        }

    }



}
