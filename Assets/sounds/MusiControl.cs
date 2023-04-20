using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusiControl : MonoBehaviour
{
    public AudioSource movement, attack, grapple;

    void Start()
    {
        //DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (!PauseMenu.GameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.A))
            {
                movement.Play();
            }
            

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                attack.Play();
            }
            

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                grapple.Play();
            }
        }
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            movement.Stop();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            attack.Stop();
        }
    }
}
