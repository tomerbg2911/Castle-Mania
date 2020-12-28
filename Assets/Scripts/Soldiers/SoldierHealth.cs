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
    private Animator anim;
    public GameObject nakedSoldier;
    SpriteRenderer[] Children;


    // Start is called before the first frame update
    void Start()
    {
        parentTower = transform.parent.GetComponent<Tower>();
        currentHealth = maxHealth;
        step = speed * Time.deltaTime;
        dead = false;
        anim = this.GetComponent<Animator>();
        Children = gameObject.GetComponentsInChildren<SpriteRenderer>();
        

    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true) // if the soldier is dead return to spawnPoint
        {
            parentTower.setGateOpen(true); // inform the tower that the soldier is dead
            nakedSoldier.transform.position = Vector3.MoveTowards(nakedSoldier.transform.position, spawnPoint.position, step);
            if (Vector3.Distance(nakedSoldier.transform.position, spawnPoint.position) < 0.1f)
            {
                // the soldier reached spawnPoint
                dead = false;
                Destroy(nakedSoldier);
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
            if(currentHealth > 0)
            {
                currentHealth += amount;
                if(currentHealth <= 0)
                {

                    transform.Rotate(transform.up, 180); // flip soldier horizontally
                    for(int i =0; i <Children.Length; i++)
                    {
                        Children[i].enabled = false;
                    }
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    nakedSoldier = Instantiate(nakedSoldier, transform.position, transform.rotation);
                    nakedSoldier.GetComponent<SpriteRenderer>().enabled = true;
                    dead = true;
                }
                if (amount <= 0)        // we are taking damage
                {
                    int num = Random.Range(1, 3);
                    switch(num)
                    {
                        case 1:         //hit shield
                            if (!anim.GetBool("Shield"))        //we didn't hit Shield yet
                            { 
                                anim.SetTrigger("hitShield");
                                anim.SetBool("Shield", true);
                            }
                            else
                            {
                                anim.SetTrigger("hitHelmet");
                            }
                            break;

                        case 2:
                            if(!anim.GetBool("Helmet"))       //we didn't hit Helmet yet
                            {
                                anim.SetTrigger("hitHelmet");
                                anim.SetBool("Helmet", true);
                            }
                            else
                            {
                                anim.SetTrigger("hitShield");
                            }
                            break;
                    }
                }
            }

            if(currentHealth >= 100)
            {
                currentHealth = 100;
            }
        }
    }

    private void DisableChildOnAnimation(int childNum)
    {
        transform.GetChild(childNum).gameObject.SetActive(false);     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Soldier"))
        {
            Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), collision.collider);
        }

    }
}
