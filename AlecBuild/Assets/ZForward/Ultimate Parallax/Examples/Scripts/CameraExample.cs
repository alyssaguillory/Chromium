using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraExample : MonoBehaviour {

    public float speed;

    private float currentSpeed;
    private Vector3 curPosition;

	void Start () {

        curPosition = transform.position;
	}
	
	
	void Update () {

        Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        currentSpeed = speed;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed *= 1.5f;
        }

        curPosition += move.normalized * currentSpeed * Time.deltaTime;
        
        transform.position = Vector3.Lerp(transform.position, curPosition, 5 * Time.deltaTime);

	}
}
