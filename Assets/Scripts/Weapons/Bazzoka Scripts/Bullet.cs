using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private  GameObject target;
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
            Debug.Log("Ive hit someone!");
            target.GetComponent<SoldierHealth>().changeHealth(-35);
            Destroy(gameObject, 0.1f);
        }

    }

    private void OnTriggerEnter2D(Collision2D collision)
    {
        target = collision.collider.gameObject;
    }
}
