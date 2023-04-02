using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInversion : MonoBehaviour
{
    private PlatformEffector2D effector;
    private float waitTime = 0.01f;
    private float baseWait;
    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        baseWait = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S))
        {
            if(waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitTime = baseWait;
            } else
            {
                waitTime -= Time.deltaTime;
            }
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            effector.rotationalOffset = 0.0f;
            waitTime = baseWait;
        }
    }
}
