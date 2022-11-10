using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotation : MonoBehaviour
{
    private float rot2;
    public float RotationSpeed;
    public bool ClockwiseRotation; 
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {
        if(ClockwiseRotation==false)
        {
            rot2 += Time.deltaTime * RotationSpeed;
        }
        else 
        {
            rot2 += -Time.deltaTime * RotationSpeed;
        }

        transform.rotation = Quaternion.Euler(0,0,rot2); 
    }
}
