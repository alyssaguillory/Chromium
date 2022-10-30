using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMoveForward : MonoBehaviour
{
    public Transform[] wheels;
    public float moveSpeed = 1.0f;
    public float accelerationTime = 1.0f;
    private float[] wheelRadius;
    void Start()
    {
        wheelRadius = new float[wheels.Length];

        for (int i = 0; i < wheels.Length; i++)
        {
            wheelRadius[i] = CameraUtils.getWidth(wheels[i].GetComponent<SpriteRenderer>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        float currentMoveSpeed = Mathf.Lerp(0, moveSpeed, Time.timeSinceLevelLoad / accelerationTime);
        float moveDelta = Time.deltaTime * currentMoveSpeed;

        transform.Translate(moveDelta, 0, 0);
        for (int i = 0; i < wheels.Length; i++)
        {
            float circumference = 2 * Mathf.PI * wheelRadius[i];

            float angle = 360.0f * moveDelta / circumference;
            wheels[i].Rotate(-Vector3.forward, angle, Space.Self);
        }
    }
}
