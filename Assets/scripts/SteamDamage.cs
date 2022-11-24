using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamDamage : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public Animator animator; 
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.tag == "Player"){
            playerDamage = collision.gameObject.GetComponent(typeof(DamageController)) as DamageController;
            Debug.Log("Player detected");
            playerDamage.Damage();
        }
        
    }
}