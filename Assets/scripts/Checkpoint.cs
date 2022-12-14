using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Checkpoint : MonoBehaviour
{
    public GameMaster gm; 
    [SerializeField] public SpriteRenderer kiosk;


  
    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        
    }
    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("Player")){
            gm.lastCheckPointPos = transform.position; 
            kiosk.color = new Color32(11,255,22,255); 
        }

    }
    // Update is called once per frame
}
