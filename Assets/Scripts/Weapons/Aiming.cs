using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : MonoBehaviour {

    //Transform weapon;
    //public float weaponRotationSpeed = 50f;
    //public float weaponMinAngle = 0f;
    //public float weaponMaxAngle = 70f;

    // keyboard keys
    private KeyCode up;
    private KeyCode down;

    public float rotationSpeed = 10f;
    public float minRotationZ = 0f;
    public float maxRotationZ = 360f;

    public Transform rotationAnchor;
    void Start()
    {
        // init keyboard keys
        up = GetComponentInParent<Tower>().up;
        down = GetComponentInParent<Tower>().down;

        //weapon = transform;
        //Debug.Log("weapon is : "+ transform.gameObject);
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
        float rotationAngleZ = (transform.eulerAngles.z > 180) ? transform.eulerAngles.z - 360 : transform.eulerAngles.z;
        if (Input.GetKey(down) && rotationAngleZ > minRotationZ)
        {
            Rotate(-transform.forward);
        }
        if (Input.GetKey(up) && rotationAngleZ < maxRotationZ)
        {
            Rotate(transform.forward);
        }
    }

    void Rotate(Vector3 axis)
    {
        transform.RotateAround(rotationAnchor.position, axis, rotationSpeed * Time.deltaTime);
    }
}
