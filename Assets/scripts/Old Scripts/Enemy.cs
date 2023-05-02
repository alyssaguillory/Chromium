using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float health, maxHealth = 3f;
    public GameObject corpse;
    AudioSource audioHurt;
    //public int maxHealth = 100;
    // public int currentHealth;
    //public int damage;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        audioHurt = GetComponent<AudioSource>();
    }

    public void TakeDamage(float damageAmount)
    {
        health -= damageAmount; // 3 -> 2 -> 1 -> 0 = Enemy has died
        if(health <= 0)
        {
            //Die();
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
        
        if (!audioHurt.isPlaying)
        {
            audioHurt.Play();
        }
        

        Debug.Log("Enemy died!");
        Instantiate(corpse, transform.position, Quaternion.identity);
        StartCoroutine(waiter());
        // Die animation
        // Disable the enemy

    }

    
    IEnumerator waiter()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
    
}
