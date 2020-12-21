using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCollectable : MonoBehaviour
{

    public int numOfMana;
    public float gravity;
    public bool isCatched;
    public Transform hookAttached;

    // Start is called before the first frame update
    void Start()
    {
        hookAttached = null;
        isCatched = false;
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    void move()
    {
        if (hookAttached == null)
        {
            transform.position -= transform.up * gravity * Time.deltaTime;
        }
        else
        {
            transform.position = hookAttached.position;
        }
    }

}
