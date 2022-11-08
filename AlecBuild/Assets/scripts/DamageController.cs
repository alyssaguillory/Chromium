using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour
{
   public Animator animator; 
    // Start is called before the first frame update
   [SerializeField] private int touchDamage;

   [SerializeField] private HealthController _healthController;

   private void OnTriggerEnter2D(Collider2D collision)
   {
        if(collision.CompareTag("Player"))
        {
            
            Damage();

        }

   }

   void Damage()
   {
    animator.SetTrigger("isHit");
    _healthController.playerHealth = _healthController.playerHealth-touchDamage;
    _healthController.UpdateHealth();
    gameObject.SetActive(false);
   }

}
