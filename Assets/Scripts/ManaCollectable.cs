using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaCollectable : MonoBehaviour
{

    public int amountOfMana = 1;
    public float fallingSpeed;
    public bool isCatched;
    public Transform hookAttached;
    public Transform anchor;
    private Vector3 distanceFromAnchor;

    // Start is called before the first frame update
    void Start()
    {
        hookAttached = null;
        isCatched = false;
        distanceFromAnchor = transform.position - anchor.position;
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
            transform.position -= transform.up * fallingSpeed * Time.deltaTime;
        }
        else
        {
            transform.position = hookAttached.position + distanceFromAnchor;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Floor")
        {
            Destroy(gameObject);
        }
    }

}
