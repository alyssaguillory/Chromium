using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth =  100.0f;
    private float CurrHealth = 100.0f;
    private float Iframe = 0.0f;
    private void OnEnable() => CurrHealth = MaxHealth;
    // Start is called before the first frame update
    public void Damage(float damage)
    {
        if (Iframe > 0)
            return;
        else
            StartCoroutine(IFrameActivator(0.2f));
        CurrHealth -= damage;
        if (CurrHealth <= 0)
            Die();
    }
    public void Die()
    {
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
}