using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookShooting : MonoBehaviour
{
    // keyboard keys
    private KeyCode shoot;
    private Hook hook;

    // Start is called before the first frame update
    void Start()
    {
        // init hook member
        hook = GetComponent<Hook>();
        // keyboard key init
        shoot = GetComponentInParent<Tower>().shoot;
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void GetInput()
    {
        if (Input.GetKeyDown(shoot) && hook.hookState == Hook.HookState.rotating)
        {
            FindObjectOfType<AudioManager>().Play("Hook Out");
            hook.Shoot();
        }
    }
}
