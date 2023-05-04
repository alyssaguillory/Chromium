using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float MaxHealth =  100.0f;
    public float CurrHealth;
    private float Iframe = 0.0f;
    public GameObject corpse;
    public SpriteRenderer[] flash;

    [SerializeField] AudioSource soundPlayer;
    [SerializeField] AudioClip hurt;
    [SerializeField] AudioClip die;
    private void OnEnable() => CurrHealth = MaxHealth;
    // Start is called before the first frame update
    private void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
    }
    public virtual void Damage(float damage, float iFrames = 0.0f)
    {
        
        if(Iframe <= 0.0f)
        {
            CurrHealth -= damage;
            if (CurrHealth <= 0)
            {
                Die();
                PlaySound(die);
            }
            else
            {
                PlaySound(hurt);
                StartCoroutine(IFrameActivator(iFrames));
                StartCoroutine(DamageFlash(iFrames));
            }
        }
        /* How to start the Iframes
        StartCoroutine(Health.IFrameActivator(0.2f));
        */
    }
    public virtual void Die()
    {
        Debug.Log("Die");
        Instantiate(corpse, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    public float GetIframes() { return Iframe; }

    public void PlaySound(AudioClip soundByte) { soundPlayer.PlayOneShot(soundByte); }

    public IEnumerator IFrameActivator(float Iframes)
    {
        Iframe = Iframes;
        while (Iframe > 0)
        {
            Iframe -= Time.deltaTime;
            yield return null;
        }
    }
    public virtual IEnumerator DamageWithInvincible(float Iframes, float damage)
    {
        if (Iframe > 0) { yield break; }
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
    public IEnumerator DamageFlash(float flashtime)
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
