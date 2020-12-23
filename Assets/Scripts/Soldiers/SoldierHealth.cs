using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierHealth : MonoBehaviour
{

    public int maxHealth;
    public int currentHealth;
    public Transform spawnPoint;    //where the player should go back to 
    public bool dead;               // if dead- return to spawnPoint
    public float speed;             //speed in which the player returns to his base
    public float step;              // calculated from speed, need it for smoothness of motion
    

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        step = speed * Time.deltaTime;
        dead = false;  
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true)
        {
            transform.position = Vector3.MoveTowards(transform.position, spawnPoint.position, step);
            if (Vector3.Distance(transform.position, spawnPoint.position) < 0.1f)
            {
                dead = false; 
            }
        }
    }

    public void changeHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth <= 0)
        {
            dead = true;
        }

        if (currentHealth >= 100)
        {
            currentHealth = 100;
        }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Soldier")
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.collider);
        }

    }



}
