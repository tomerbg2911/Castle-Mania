using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int healthPoints = 100;
    public bool isGateOpen = false;

    // soldiers related vars
    public int maxNumOfSoldiers = 5;
    public int currentNumOfSoldiers = 5;

    // keyboard keys
    public KeyCode up;
    public KeyCode down;
    public KeyCode shoot;
    public KeyCode switchWeapon;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
