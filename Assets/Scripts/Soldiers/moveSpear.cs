using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class moveSpear : MonoBehaviour
{

    public Transform target;            //holds the rotation value we want the spear to end up
    private float rotateSpeed;
    public Rigidbody2D rb;
    public string tower;
    bool move = true;


    // vars to add variance in final angle of the Spear
    private GameObject finalRotation;
    private float offsetFinalRotation;
    public float minOffset= -10f;
    public float maxOffset= 10f;
    public float minRotateSpeed = 40;
    public float maxRotateSpeed = 70;

    // Start is called before the first frame update

    //private void Awake()
  //  {
    //    sr = gameObject.AddComponent<SpriteRenderer>;
   // }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find(string.Format("Ground{0}",tower)).transform;
        finalRotation = new GameObject();
        Debug.Log("target is : " + target.name);
        rotateSpeed = Random.Range(minRotateSpeed, maxRotateSpeed);
        offsetFinalRotation = Random.Range(minOffset, maxOffset);
        finalRotation.transform.position = target.transform.position;
        finalRotation.transform.rotation = target.transform.rotation;
        finalRotation.transform.Rotate(0, 0, offsetFinalRotation);
        Destroy(gameObject, 4);
    }

    private void FixedUpdate()
    {
        if (move)
        {
            var step = rotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, finalRotation.transform.rotation, step);
            
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        move = false;
        rb.simulated = false;
        Destroy(finalRotation);
    }

    // Update is called once per frame
    /* void FixedUpdate()
     {
         Vector3 moveDirection = rb.velocity;
         if (moveDirection != Vector3.zero)
         {
             float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
             transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
         }
     }
    */
}
