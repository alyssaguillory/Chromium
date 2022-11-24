using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SteamDamage : MonoBehaviour
{
    //public _health playerDamage;
    [SerializeField] private HealthController health;
    [SerializeField] private int touchDamage;
    
    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player"){
            health = collision.gameObject.GetComponent(typeof(HealthController)) as HealthController;
            Debug.Log("Player detected");
            damage(health);
        }
        
    }
    void damage(HealthController health) {
    //animator.SetTrigger("isHit");
    health.playerHealth = health.playerHealth-touchDamage;
    health.UpdateHealth();
    if(health.playerHealth == 0){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    //gameObject.SetActive(false);
   }
}