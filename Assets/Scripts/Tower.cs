using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Tower : MonoBehaviour
{
    public int playerNumber;
    public int healthPoints = 100;

    // gate related vars
    private GameObject gateOpen;
    private GameObject gateClosed;
    public bool isGateOpen;
    public float delayBeforeClosingTheGate = 2f; // after a new soldier was Instantiated
    public float delayBeforeInstantiating = 0.5f;  // before a new soldier is Instantiated

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


    // Start is called before the first frame update
    void Start()
    {
        // init gates GameObjects
        GameObject gateParent = transform.Find("gate").gameObject;
        gateOpen = gateParent.transform.Find("gate_open").gameObject;
        gateClosed = gateParent.transform.Find("gate_closed").gameObject;
        setGateOpen(false);
        

        // spawn first soldiers
        for (int i = 0; i < maxNumOfSoldiers; i++)
        {
            StartCoroutine(instantiateSoldierEnumerator((i + 1) * delayBeforeInstantiating));
        }
    }

    IEnumerator instantiateSoldierEnumerator(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        InstantiateSoldier();
    }

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
            newSoldierComponent.speed = 10;

            currentNumOfSoldiers++;
            countNumOfSoldiers++;

            Invoke("closeGate", delayBeforeClosingTheGate);
        }
    }
    public void setGateOpen(bool value)
    {
        isGateOpen = value;
        gateOpen.SetActive(isGateOpen);
        gateClosed.SetActive(!isGateOpen);
    }

    // TODO: those functions are for using it with Invoke. is there a better way to run func. after delay? (Coroutines?)
    void openGate()
    {
        setGateOpen(true);
    }
    void closeGate()
    {
        setGateOpen(false);
    }


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


}
