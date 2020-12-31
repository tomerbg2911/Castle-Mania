using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowWeapon : MonoBehaviour
{
    public Rigidbody2D spear;
    public GameObject parentWeapon;
    private IEnumerator throwRunning;
    


    //vars to make the timing and force of the throw random
    public int minSeconds = 5;
    public int maxSeconds = 10;
    public int minForce = 45;
    public int maxForce = 60;
    
    // Start is called before the first frame update
    void Start()
    {
       parentWeapon = gameObject.transform.Find("Weapon").gameObject;
        
        
       //throwAfterXsec(4f, 30f);
    }

    // Update is called once per frame
    void Update()
    {
        if (throwRunning == null)
        {
           int seconds = Random.Range(minSeconds, maxSeconds);
           int  force = Random.Range(minForce,maxForce);
           StartCoroutine(throwAfterXsec(seconds, force));
            throwRunning = throwAfterXsec(seconds,force); 

        }
       

    }

    public void throwSpear(float force)
    {
        parentWeapon.transform.Rotate(0f, 1f, 45.0f);
        Rigidbody2D spearInstance = Instantiate(spear, parentWeapon.transform.position, parentWeapon.transform.rotation) as Rigidbody2D;
        spearInstance.velocity = force * parentWeapon.transform.up;
        



    }

    public IEnumerator throwAfterXsec(float sec, float force)
    {
       yield return new WaitForSeconds(sec);
       throwSpear(force);
       parentWeapon.SetActive(false);
       yield return new WaitForSeconds(4);
       parentWeapon.transform.Rotate(0f, -1f, -45.0f);
       parentWeapon.SetActive(true);
       Debug.Log("Ive thrown");
       throwRunning = null;
    }

   





}
