using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    public Rigidbody2D spear;
    public Transform FireTransform;
    private GameObject parentWeapon;
    private Rigidbody2D rb;
    private float turnSpeed = 5f;
    // Start is called before the first frame update
    void Start()
    {
        parentWeapon = gameObject.transform.Find("Weapon").gameObject;
        StartCoroutine(throwAfterXsec(5));


    }

    // Update is called once per frame
    void Update()
    {

    }

    public void throwSpear(float force)
    {
        parentWeapon.transform.Rotate(0f, 1f, 45.0f);
        Rigidbody2D spearInstance = Instantiate(spear, parentWeapon.transform.position, parentWeapon.transform.rotation) as Rigidbody2D;
        Debug.Log("parentWeapon.transform.up: " + parentWeapon.transform.up);
        //spearInstance.velocity = force * parentWeapon.transform.up;
        

    }

    IEnumerator throwAfterXsec(float sec)
    {
       yield return new WaitForSeconds(sec);
        throwSpear(25f);
        Debug.Log("Ive thrown");
    }

   





}
