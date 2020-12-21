using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{

    public int playerNumber = 1;
    public Rigidbody2D bullet;
    public Transform FireTransform;
    public Slider AimSlider;
    public AudioSource ShootingAudio;
    public AudioClip ChargingClip;
    public AudioClip fireClip;
    public float MinLaunchForce = 15f;
    public float MaxLaunchForce = 30f;
    public float MaxChargeTime = 0.75f;

    private KeyCode FireButton;
    private float currentLaunchForce;
    private float ChargeSpeed;  
    private bool Fired;


    // Start is called before the first frame update
    void Start()
    {
        FireButton = GetComponentInParent<Tower>().shoot;
        ChargeSpeed = (MaxLaunchForce - MinLaunchForce) / MaxChargeTime;
        AimSlider.gameObject.SetActive(false);
        // Debug.Log("FireButton" + FireButton);
    }

    // Update is called once per frame
    void Update()
    {
        AimSlider.value = MinLaunchForce;
        if (currentLaunchForce >= MaxLaunchForce && !Fired)
        {
            currentLaunchForce = MaxLaunchForce;
            Fire();
        }
        else if (Input.GetKeyDown(FireButton))
        {
            Fired = false;
            currentLaunchForce = MinLaunchForce;
            AimSlider.gameObject.SetActive(true);
            // ShootingAudio.clip = ChargingClip;
            // ShootingAudio.Play();
        }
        else if (Input.GetKey(FireButton) && !Fired)
        {
            currentLaunchForce += ChargeSpeed * Time.deltaTime;
            //Debug.Log("Current launch force: " + currentLaunchForce);
            AimSlider.value = currentLaunchForce;
           
        }
        else if (Input.GetKeyUp(FireButton) && !Fired)
        {
            Fire();
          //  Debug.Log("Current launch force: " + currentLaunchForce);
        }

    }

    private void Fire()
    {
        Fired = true;
        Rigidbody2D bulletInstance = Instantiate(bullet, FireTransform.position, FireTransform.rotation) as Rigidbody2D;
        bulletInstance.velocity = currentLaunchForce * FireTransform.up;

        //ShootingAudio.clip = fireClip;
        //ShootingAudio.Play();

        currentLaunchForce = MinLaunchForce;
        AimSlider.gameObject.SetActive(false);
        

    }
}
