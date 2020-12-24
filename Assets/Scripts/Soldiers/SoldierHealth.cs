using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierHealth : MonoBehaviour
{

    public Transform spawnPoint;    // where the player should go back to
    public SoldierSlot soldierSlot;  // where the player should go towards 
    public int maxHealth;
    public int currentHealth;
    public bool dead;               // if dead - return to spawnPoint
    public float speed;             // speed in which the player returns to his base
    public float step;              // calculated from speed, need it for smoothness of motion
    
    private Tower parentTower;
    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        parentTower = transform.parent.GetComponent<Tower>();
        currentHealth = maxHealth;
        step = speed * Time.deltaTime;
        dead = false;  
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true) // if the soldier is dead return to spawnPoint
        {
            parentTower.setGateOpen(true); // inform the tower that the soldier is dead
            transform.position = Vector3.MoveTowards(transform.position, spawnPoint.position, step);
            if (Vector3.Distance(transform.position, spawnPoint.position) < 0.1f)
            {
                // the soldier reached spawnPoint
                dead = false;
                Destroy(gameObject);
                parentTower.OnSoldierIsBack(soldierSlot);
            }
        }
        else // move towards soldierSlot
        {
            transform.position = Vector3.MoveTowards(transform.position, soldierSlot.transform.position, step);
        }
    }

    public void changeHealth(int amount)
    {
        if(!dead)
        {
            currentHealth += amount;
            if(currentHealth <= 0)
            {
                dead = true;
                transform.Rotate(transform.up, 180); // flip soldier horizontally
            }

            if(currentHealth >= 100)
            {
                currentHealth = 100;
            }
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
