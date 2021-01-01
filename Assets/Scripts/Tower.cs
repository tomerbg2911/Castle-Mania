using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Tower : MonoBehaviour
{
    public int playerNumber;
    public int healthPoints = 100;
    public int manaAmount = 0;

    //UI related vars
    public HealthBar healthbar;
    public ManaBar manabar;
    public GameOverMenu gameover;

    // gate related vars
    private GameObject gateOpen;
    private GameObject gateClosed;
    //public GameObject explosionPrefab;
    public bool isGateOpen;
    public float gateIsOpenDelay = 10f; // time delay for the gate to be open after a soldier died
    public float delayBeforeInstantiating = 0.5f;  // before a new soldier is Instantiated
    private float numOfDeadSoldiers;

    // soldiers related vars
    public GameObject soldierPrefab;
    public Transform soldierInstantiatePosition;
    public SoldierSlot[] soldierSlots;
    public int maxNumOfSoldiers = 3;
    public int currentNumOfSoldiers = 0;
    private int countNumOfSoldiers = 0;

    // keyboard keys
    public KeyCode up;
    public KeyCode down;
    public KeyCode shoot;
    public KeyCode switchWeapon;

    // weapons game objects
    public GameObject[] weapons;
    private int activatedWeaponIdx;

    // weapons indicator
    public Transform[] HookIndicators;
    public Transform BazookaIndicator;
    public Transform DragonIndicator;


    void Start()
    {
        // activate weapon
        activatedWeaponIdx = 0;
        EnableActivatedWeapon();

        // init gates GameObjects
        GameObject gateParent = transform.Find("gate").gameObject;
        gateOpen = gateParent.transform.Find("gate_open").gameObject;
        gateClosed = gateParent.transform.Find("gate_closed").gameObject;
        setGateOpen(false);

        //init UI elements
        healthbar = GameObject.Find(string.Format("HealthBar{0}", playerNumber)).GetComponent<HealthBar>();
        healthbar.SetHealth(this.healthPoints);
        manabar = GameObject.Find(string.Format("ManaBar{0}", playerNumber)).GetComponent<ManaBar>();
        manabar.SetMana(0);
        gameover = GameObject.Find("Canvas").GetComponent<GameOverMenu>();

        // spawn first soldiers
        numOfDeadSoldiers = 0;
        for (int i = 0; i < maxNumOfSoldiers; i++)
        {
            StartCoroutine(instantiateSoldierEnumerator((i + 1) * delayBeforeInstantiating));
        }

        // set manaAmount amount to 0
        manaAmount = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(switchWeapon))
        {
            SwitchToNextWeapon();
        }

        setGateOpen(numOfDeadSoldiers > 0);
    }

    public void SwitchToNextWeapon()
    {
        // check if switching is possible
        if (weapons[activatedWeaponIdx].name.ToLower() == "hook" && weapons[activatedWeaponIdx].GetComponent<Hook>().hookState != Hook.HookState.rotating) // hook is busy
            return;
        if (weapons[activatedWeaponIdx].name.ToLower() == "dragon" && weapons[activatedWeaponIdx].GetComponent<DragonAiming>().isAiming) // dragon is busy
            return;

        // switch weapon idx
        activatedWeaponIdx = (activatedWeaponIdx + 1) % weapons.Length; // update the activated weapon

        // skip dragon weapon if there's not enough manaAmount
        if (weapons[activatedWeaponIdx].name.ToLower() == "dragon" &&
            manaAmount < weapons[activatedWeaponIdx].GetComponent<DragonAiming>().minAmountOfMana)
        {
            activatedWeaponIdx = (activatedWeaponIdx + 1) % weapons.Length;
        }

        EnableActivatedWeapon();
    }

    void EnableActivatedWeapon()
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            GameObject currentWeapon = weapons[i];
            bool enableThisWeapon = i == activatedWeaponIdx;
            switch (currentWeapon.name.ToLower())
            {
                case "hook":
                    foreach(Transform hookIndicator in HookIndicators)
                    {
                        hookIndicator.GetComponent<SpriteRenderer>().enabled = enableThisWeapon;
                    }
                    currentWeapon.GetComponent<Aiming>().enabled = enableThisWeapon;
                    currentWeapon.GetComponent<HookShooting>().enabled = enableThisWeapon;
                    break;
                case "bazooka":
                    BazookaIndicator.GetComponent<SpriteRenderer>().enabled = enableThisWeapon;
                    currentWeapon.GetComponent<Aiming>().enabled = enableThisWeapon;
                    currentWeapon.GetComponent<Shooting>().enabled = enableThisWeapon;
                    break;
                case "dragon":
                    DragonIndicator.GetComponent<SpriteRenderer>().enabled = enableThisWeapon;
                    currentWeapon.GetComponent<DragonAiming>().enabled = enableThisWeapon;
                    break;
            }
        }
    }


    // instantiate a new soldier after waitingTime seconds
    IEnumerator instantiateSoldierEnumerator(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        // FindObjectOfType<AudioManager>().Play("Soldier Out");
        InstantiateSoldier();
    }


    // instantiate a new soldier
    public void InstantiateSoldier()
    {
        SoldierSlot availableSlot = getAvailableSlot();

        // no place was found
        if (availableSlot == null)
        {
            print(string.Format("Tower {0} - No free slots", playerNumber));
        }

        // instantiate a new soldier and target it towards the free soldier slot
        else
        {
            setGateOpen(true);

            GameObject newSoldier = Instantiate(soldierPrefab,
                                        soldierInstantiatePosition.position,
                                        Quaternion.LookRotation(-transform.forward, transform.up),
                                        transform) as GameObject;
            newSoldier.GetComponent<SortingGroup>().sortingOrder = countNumOfSoldiers;
            SoldierHealth newSoldierComponent = newSoldier.GetComponent<SoldierHealth>();
            newSoldierComponent.spawnPoint = soldierInstantiatePosition;
            newSoldierComponent.soldierSlot = availableSlot;
            //newSoldierComponent.speed = 10;

            currentNumOfSoldiers++;
            countNumOfSoldiers++;

            //Invoke("closeGate", delayBeforeClosingTheGate);
        }
    }
    public void setGateOpen(bool value)
    {
        isGateOpen = value;
        gateOpen.SetActive(isGateOpen);
        gateClosed.SetActive(!isGateOpen);
    }

    //public void openGate()
    //{
    //    setGateOpen(true);
    //}
    //public void closeGate()
    //{
    //    setGateOpen(false);
    //}

    // returns a free SoldierSlot or null if it's all taken
    SoldierSlot getAvailableSlot()
    {
        // search for a free soldier slot
        SoldierSlot availableSlot = null;
        foreach (SoldierSlot slot in soldierSlots)
        {
            if (slot.isFree)
            {
                availableSlot = slot;
                slot.isFree = false; // mark the spot as taken
                break;
            }
        }

        return availableSlot;
    }

    // Tower event handlers
    public void onDragonFireHitGate()
    {
        if (isGateOpen)   //here we hit the other tower, need to get his health and update it, the other tower health slider
        {
            this.healthPoints -= 10;
            healthbar.SetHealth(this.healthPoints);
            FindObjectOfType<AudioManager>().Play(string.Format("SABA{0} hit", playerNumber == 1 ? " II" : "")); // play wizard hit sound
            GetComponent<Animator>().SetTrigger("Got Hit"); // play animation
            print(string.Format("tower {0} got hit", playerNumber));
            Debug.Log("current health:" + this.healthPoints);

            if (this.healthPoints <= 0)
            {
                gameover.playerIsDead = true;
            }

            // show explosion animation
           // GameObject explosion = Instantiate(explosionPrefab, gateOpen.transform) as GameObject;
            //Destroy(explosion, 3);
        }
        else
        {
            print(string.Format("tower {0} miss", playerNumber));

            // show explosion animation
           // GameObject explosion = Instantiate(explosionPrefab, gateOpen.transform.position, Quaternion.identity) as GameObject;
           // Destroy(explosion, 3);
        }
    }

    // activated after a soldier came back to the tower's gate gate
    public void OnSoldierIsBack(SoldierSlot soldierSlotToFree)
    {
        currentNumOfSoldiers--;

        // free the soldierSlot
        foreach (SoldierSlot slot in soldierSlots)
        {
            if (slot == soldierSlotToFree)
            {
                slot.isFree = true;
            }
        }
        Invoke("InstantiateSoldier", delayBeforeInstantiating);
    }

    public void OnSoldierIsDead()
    {
        numOfDeadSoldiers++;
        setGateOpen(true);
        StartCoroutine(reduceNumOfDeadSoldiersEnumerator(gateIsOpenDelay));
    }

    IEnumerator reduceNumOfDeadSoldiersEnumerator(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        numOfDeadSoldiers--;
        print("num of dead soldiers is " + numOfDeadSoldiers);
    }


    // activated after the hook fame back to its base with a collectable
    public void OnCollectableCatch(GameObject collectableGameObject)
    {
        ManaCollectable manaCollectable;
        if ((manaCollectable = collectableGameObject.GetComponent<ManaCollectable>()) != null)
        {
            setManaAmount(manaAmount + manaCollectable.amountOfMana);
            if ((manaAmount >= 1 && manaAmount <= 3) && playerNumber == 1)
            {
                FindObjectOfType<AudioManager>().Play("SABA - " + manaAmount);
            }
            if ((manaAmount >= 1 && manaAmount <= 3) && playerNumber == 2)
            {
                FindObjectOfType<AudioManager>().Play("SABA II - " + manaAmount);
            }
        }

        Destroy(collectableGameObject);
    }

    public void setManaAmount(int amount)
    {
        manaAmount = amount;
        manabar.SetMana(amount);
    }

}
