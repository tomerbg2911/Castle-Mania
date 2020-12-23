using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int healthPoints = 100;

    // gate related vars
    private GameObject gateOpen;
    private GameObject gateClosed;
    public bool isGateOpen = false;

    // soldiers related vars
    public GameObject soldierPrefab;
    public Transform soldierInstantiatePosition;
    public int maxNumOfSoldiers = 5;
    public int currentNumOfSoldiers = 5;

    public Transform[] soldierSlots;
    private int nextSlotIdx;


    // keyboard keys
    public KeyCode up;
    public KeyCode down;
    public KeyCode shoot;
    public KeyCode switchWeapon;


    // Start is called before the first frame update
    void Start()
    {
        GameObject gateParent = transform.Find("gate").gameObject;
        gateOpen = gateParent.transform.Find("gate_open").gameObject;
        gateClosed = gateParent.transform.Find("gate_open").gameObject;

        nextSlotIdx = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            InstantiateSoldier();           
    }

    void setGateOpen(bool value)
    {
        isGateOpen = value;
        //gateOpen.SetActive();
        //gate.GetComponent<SpriteRenderer>().sprite = value ? gateOpenSprite : gateClosedSprite;
    }

    void InstantiateSoldier()
    {
        setGateOpen(true);

        GameObject newSoldier = Instantiate(soldierPrefab,
            soldierInstantiatePosition.position, 
            Quaternion.LookRotation(-transform.forward, transform.up),
            transform) as GameObject;
        Soldier newSoldierComponent = newSoldier.GetComponent<Soldier>();
        newSoldierComponent.target = soldierSlots[nextSlotIdx];
        nextSlotIdx = (nextSlotIdx + 1) % soldierSlots.Length;
        newSoldierComponent.movingSpeed = 10;
    }
}
