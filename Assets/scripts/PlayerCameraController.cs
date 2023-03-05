using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [SerializeField] private Vector2 offset;
    public GameObject player;
    public float offsetSmoothing;
    [SerializeField] private Vector3 wantedPosition;
    [SerializeField] private float maxDist;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftControl)) {
            Vector2 destiny = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            wantedPosition = new Vector3((player.transform.position.x + destiny.x)/2, (player.transform.position.y + destiny.y) / 2, -10);
        }
        else { wantedPosition = new Vector3(player.transform.position.x + offset.x, player.transform.position.y + offset.y, -10); }
        /*
        if(player.transform.localScale.x > 0)
        {
            playerPosition = new Vector3(playerPosition.x + offset, playerPosition.y, playerPosition.z);
        } else {
            playerPosition = new Vector3(playerPosition.x - offset, playerPosition.y, playerPosition.z);

        }
        */
        transform.position = Vector3.Lerp(transform.position, wantedPosition, offsetSmoothing * Time.deltaTime);
    }
}
