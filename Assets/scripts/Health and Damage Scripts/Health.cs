using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth =  100.0f;
    public float CurrHealth;
    public int HealthBars = 1;
    private float Iframe = 0.0f;
    public GameObject corpse;
    public SpriteRenderer[] flash;
    private void OnEnable() => CurrHealth = MaxHealth;
    // Start is called before the first frame update
    private void Start()
    {
        CurrHealth = MaxHealth;
    }
    public virtual void Damage(float damage)
    {
        Debug.Log("MainDamage");
        /* How to start the Iframes
        StartCoroutine(Health.IFrameActivator(0.2f));
        */
        CurrHealth -= damage;
        if (CurrHealth <= 0)
            Die();
    }
    public void Die()
    {
        Destroy(gameObject);
        Instantiate(corpse, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public float GetIframes() { return Iframe; }

    public IEnumerator IFrameActivator(float Iframes)
    {
        Iframe = Iframes;
        while (Iframe > 0)
        {
            Iframe -= Time.deltaTime;
            yield return null;
        }
    }
    public IEnumerator DamageWithInvincible(float Iframes, float damage)
    {
        Damage(damage);
        Iframe = Iframes;
        StartCoroutine(DamageFlash(Iframes));
        while (Iframe > 0)
        {
            //Debug.Log("Invincibility Count Down");
            //Debug.Log(Iframe);
            Iframe -= Time.deltaTime;
            yield return null;
        }
        
    }
    IEnumerator DamageFlash(float flashtime)
    {
        Color flashHalfColor = new Color(1.0f, 1.0f, 1.0f, 0.25f);
        for (int i = 0; i < 4; i++)
        {
            foreach (SpriteRenderer spriteColor in flash) { spriteColor.color = flashHalfColor; }
            yield return new WaitForSeconds(flashtime / 8);
            foreach (SpriteRenderer spriteColor in flash) { spriteColor.color = Color.white; }
            yield return new WaitForSeconds(flashtime / 8);
        }
    }
}
