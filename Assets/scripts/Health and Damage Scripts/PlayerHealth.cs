using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    [SerializeField] GearSpawner healthSpawner;
    //[SerializeField] CanvasGroup canvasGroup;
    public int HealthBars = 1;
    public int CurrBars;
    public float regenWait;
    IEnumerator regen;
    public GameMaster gm;
    //AudioSource[] audioData;
    //AudioSource audioHurt;
    //AudioSource audioDie;

    private void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        if (gm.bars != null)
            HealthBars = gm.bars;
        if (HealthBars == 0)
            HealthBars = 1;
        CurrBars = HealthBars;
        while (healthSpawner.Gears.Count < HealthBars) { healthSpawner.AddBar(); };
        

        //audioData = GetComponents<AudioSource>();
        //audioHurt = audioData[0];
        //audioDie = audioData[1];

    }
    public override void Die()
    {
        if (CurrBars > 1)
        {
            CurrBars--;
            CurrHealth = MaxHealth;
            healthSpawner.redoHealth();
        }
        else {
            StartCoroutine(waiter());
            //transform.position = gm.lastCheckPointPos;
        }
        
        Debug.Log("You Died");
    }
    public override void Damage(float damage, float iFrames = 0.0f)
    {
        if (regen != null) { StopCoroutine(regen); }
        regen = regenarate(regenWait);
        StartCoroutine(regen);
        base.Damage(damage, iFrames);
    }
    IEnumerator regenarate(float regenTimer)
    {
        while (regenTimer > 0)
        {
            regenTimer -= Time.deltaTime;
            yield return null;
        }
        while (CurrHealth < MaxHealth)
        {
            CurrHealth += 0.1f;
            if(CurrHealth>MaxHealth) { CurrHealth = MaxHealth; }
            yield return null;
        }
    }
    void OnParticleCollision(GameObject other)
    {
        //Debug.Log("Hit");
        //audioHurt.Play();
        Damage(3.0f, 0.1f);
    }

    public void AddBar() { HealthBars++; CurrBars++; healthSpawner.AddBar(); }

    IEnumerator waiter()
    {
        /*
        if (!audioDie.isPlaying)
        {
            audioDie.Play();
        } */
        GameObject.Find("CanvasGroup").GetComponent<FadeScript>().ShowUI();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}

