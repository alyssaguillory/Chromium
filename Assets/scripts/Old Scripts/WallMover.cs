using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMover : MonoBehaviour
{
    [SerializeField] GameObject wall;
    [SerializeField] BossController boss;
    [SerializeField] Collider2D inviswall;
    public bool active = false;
    public Vector2 current;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Triggered");
        if(active == false)
        {
            boss.advanceGameStage();
            StartCoroutine(wallDrop());
            active = true;
        }
    }
    IEnumerator wallDrop()
    {
        //7.12
        inviswall.enabled = true;
        while(wall.transform.localPosition.y > 7.12)
        {
            current = wall.transform.position;
            wall.transform.localPosition = wall.transform.localPosition - new Vector3(0.0f, 15.0f, 0.0f) * Time.deltaTime;
            yield return null;
        }
        
    }
}
