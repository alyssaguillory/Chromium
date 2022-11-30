using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SteamDamage : MonoBehaviour
{
    //public _health playerDamage;
    [SerializeField] private PlayerHealth health;
    [SerializeField] private float touchDamage;
    
    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Steam"){
            //health = collision.gameObject.GetComponent(typeof(PlayerHealth)) as PlayerHealth;
            Debug.Log("Player detected");
            damage();
        }
        
    }
    void damage() {
    //health.playerHealth = health.playerHealth-touchDamage;
    health.Damage(touchDamage);
    //if(health.playerHealth == 0){
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //}
    //gameObject.SetActive(false);
   }
}