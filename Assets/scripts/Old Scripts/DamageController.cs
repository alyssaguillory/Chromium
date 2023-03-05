using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DamageController : MonoBehaviour
{
   public Animator animator;
    private float timer = 0.25f;
    // Start is called before the first frame update
   [SerializeField] private int touchDamage;

   [SerializeField] private HealthController _healthController;

   private void OnTriggerEnter2D(Collider2D collision)
   {
        timer -= Time.deltaTime;
        if(collision.CompareTag("Player"))
        {
            timer = 0.25f;
            Damage();

        }
        

   }

   void Damage()
   {
    //animator.SetTrigger("isHit");
    _healthController.playerHealth = _healthController.playerHealth-touchDamage;
    _healthController.UpdateHealth();
    if(_healthController.playerHealth == 0){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
        //gameObject.SetActive(false);
    }

}
