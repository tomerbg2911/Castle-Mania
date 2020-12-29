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
    public Transform animationPivot;
    public GameObject nakedSoldier;
    public GameObject armoredSoldier;
    public GameObject poofPrefab;
    SpriteRenderer[] Children;


    // Start is called before the first frame update
    void Start()
    {
        parentTower = transform.parent.GetComponent<Tower>();
        currentHealth = maxHealth;
        step = speed * Time.deltaTime;
        dead = false;
        anim = animationPivot.GetComponent<Animator>();
        Children = gameObject.GetComponentsInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (dead == true) // if the soldier is dead return to spawnPoint
        {
            nakedSoldier.transform.position = Vector3.MoveTowards(nakedSoldier.transform.position, spawnPoint.position, step);
            if (Vector3.Distance(nakedSoldier.transform.position, spawnPoint.position) < 0.1f)
            {
                // the soldier reached spawnPoint
                //dead = false;
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
                    Debug.Log("iM dead yall");
                    transform.Rotate(transform.up, 180); // flip soldier horizontally
                    for(int i =0; i <Children.Length; i++)
                    {
                        Children[i].enabled = false;
                    }
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    nakedSoldier = Instantiate(nakedSoldier, transform.position, transform.rotation);
                    nakedSoldier.GetComponent<SpriteRenderer>().enabled = true;
                    parentTower.OnSoldierIsDead(); // inform tower that the soldier is dead
                    dead = true;
                }
                else if  (amount <= 0)        // we are taking damage
                {
                    if (gameObject.name.ToLower().StartsWith("armor"))
                    {
                        armorHit();
                    }
                    else
                    {
                        hit();
                    }
                }
            }

            if(currentHealth >= 100)
            {
                currentHealth = 100;
            }
        }
    }


    private void hit()              // play a random hit animation for regular soldier
    {
        int num;
        bool hit = false;
        while (!hit)
        {
            num = Random.Range(1, 3);
            switch (num)
            {
                case 1:         //hit shield
                    if (!anim.GetBool("Shield"))        //we didn't hit Shield yet
                    {
                        anim.SetTrigger("hitShield");
                        anim.SetBool("Shield", true);
                        hit = true;
                    }
                    break;

                case 2:
                    if (!anim.GetBool("Helmet"))       //we didn't hit Helmet yet
                    {
                        anim.SetTrigger("hitHelmet");
                        anim.SetBool("Helmet", true);
                        hit = true;
                    }
                    break;
            }
        }
    }

    private void armorHit()              // play a random hit animation for armored soldier
    {
        int num = Random.Range(1, 4);
        bool hit = false;
        while (!hit)
        {
            num = Random.Range(1, 4);
            switch (num)
            {
                case 1:         //hit shield
                    if (!anim.GetBool("Sword"))        //we didn't hit Shield yet
                    {
                        anim.SetTrigger("hitSword");
                        anim.SetBool("Sword", true);
                        hit = true;
                    }
                    break;

                case 2:
                    if (!anim.GetBool("Helmet"))       //we didn't hit Helmet yet
                    {
                        anim.SetTrigger("hitHelmet");
                        anim.SetBool("Helmet", true);
                        hit = true;
                    }
                    break;

                case 3:
                    if (!anim.GetBool("Shoulder"))      // we didn't hit shoulder yet
                    {
                        anim.SetTrigger("hitShoulder");
                        anim.SetBool("Shoulder", true);
                        hit = true;
                    }
                    break;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name.ToLower().StartsWith("shield"))
        {
            print("I GOT SHIELD!"); // TODO: Implement shield animation

                                                                         
            if (!(gameObject.name.ToLower().StartsWith("armor")))               // replacing reg-soldier with armed soldier
            {
                StartCoroutine(instantiateArmoredSoldierEnumerator(0.3f));
                Instantiate(poofPrefab, transform.position, Quaternion.identity);

                //destroy regular soldier
                Destroy(gameObject, 0.5f);
            }

            Collectable shieldCollectable = collision.gameObject.GetComponentInParent<Collectable>();

            // tell hook to go back
            GameObject hookAnchorAttached = shieldCollectable.hookAnchorAttached.gameObject;
            hookAnchorAttached.GetComponentInParent<Hook>().hookState = Hook.HookState.firedGoingBack;

            // destroy shield game object
            Destroy(shieldCollectable.gameObject, 0.1f);

        }
    }

    // instantiate a new armored soldier after waitingTime seconds
    IEnumerator instantiateArmoredSoldierEnumerator(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        armoredSoldier = Instantiate(armoredSoldier, transform.position, transform.rotation);
        armoredSoldier.transform.parent = this.transform.parent;
        armoredSoldier.GetComponent<SoldierHealth>().spawnPoint = this.spawnPoint;
        armoredSoldier.GetComponent<SoldierHealth>().soldierSlot = this.soldierSlot;
        armoredSoldier.GetComponent<SoldierHealth>().Children = armoredSoldier.GetComponentsInChildren<SpriteRenderer>();
    }

}
