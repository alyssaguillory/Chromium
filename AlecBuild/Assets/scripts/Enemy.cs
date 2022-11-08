using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;
    //public int maxHealth = 100;
   // public int currentHealth;
    //public int damage;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        
    }
    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount; // 3 -> 2 -> 1 -> 0 = Enemy has died
        if(health <= 0)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        // Play hurt animation
        if(health <= 0)
        {
            Die();
        }
        
    }
    void Die()
    {
        Debug.Log("Enemy died!");

        // Die animation
        // Disable the enemy

    }
}
