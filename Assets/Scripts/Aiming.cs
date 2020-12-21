using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour {
    Transform weapon;
    public float weaponRotationSpeed = 50f;
    public float weaponMinAngle = 0f;
    public float weaponMaxAngle = 70f;

    private KeyCode up;
    private KeyCode down;
    private KeyCode shoot;
    private KeyCode switchWeapon;

    public float rotationSpeed = 10f;
    public float minRotationZ = 0f;
    public float maxRotationZ = 360f;

    public Transform rotationAnchor;
    void Start()
    {
        weapon =  GameObject.Find("Bazooka").transform;
        Debug.Log("weapon is : "+ weapon.gameObject);
        up = GetComponentInParent<Tower>().up;
        down = GetComponentInParent<Tower>().down;
        shoot = GetComponentInParent<Tower>().shoot;
        switchWeapon = GetComponentInParent<Tower>().switchWeapon;
        //Debug.Log("up key is : " + up);
    }

     void Update()
    {
        GetInput();
     }

    /*void RotateWeapon(float axis)
   {
        float rot = weapon.rotation.eulerAngles.z ;
        var rotationTarget = Mathf.Clamp(rot - axis, weaponMinAngle, weaponMaxAngle);
        rot = Mathf.MoveTowardsAngle(rot, rotationTarget, Time.deltaTime * weaponRotationSpeed);
        weapon.rotation = Quaternion.Euler(0f,0f,rot);

    }
    */

    void GetInput()
    {
        if (Input.GetKey(down) && transform.eulerAngles.z > minRotationZ)
        {
            
            Rotate(-transform.forward);
        }
        if (Input.GetKey(up) && transform.eulerAngles.z < maxRotationZ)
        {
            Rotate(transform.forward);
        }
      
    }

    void Rotate(Vector3 axis)
    {
       // if (hookState == HookState.rotating)
       // {
            transform.RotateAround(rotationAnchor.position, axis, rotationSpeed * Time.deltaTime);
        //}
    }
}
