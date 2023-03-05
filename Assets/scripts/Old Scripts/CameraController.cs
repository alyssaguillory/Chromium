using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    public float Xoffset;
    public float Yoffset;
    public float offsetSmoothing;
    private Vector3 playerPosition;
    public GameObject MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerPosition = new Vector3(player.transform.position.x + Xoffset,player.transform.position.y + Yoffset,MainCamera.transform.position.z);
        /*
        if(player.transform.localScale.x > 0)
        {
            playerPosition = new Vector3(playerPosition.x + offset, playerPosition.y, playerPosition.z);
        } else {
            playerPosition = new Vector3(playerPosition.x - offset, playerPosition.y, playerPosition.z);

        }
        */
        MainCamera.transform.position = Vector3.Lerp(transform.position, playerPosition,offsetSmoothing *Time.deltaTime);
    }
}
