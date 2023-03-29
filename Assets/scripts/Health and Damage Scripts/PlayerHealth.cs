using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : Health
{
    [SerializeField] GearSpawner healthSpawner;
    public int HealthBars = 1;
    public int CurrBars;
    public float regenWait;
    IEnumerator regen;
    public GameMaster gm;
    private void Start()
    {
        CurrBars = HealthBars;
        while (healthSpawner.Gears.Count < HealthBars) { healthSpawner.AddBar(); };
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
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
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            transform.position = gm.lastCheckPointPos;
        }
        Debug.Log("You Died");
    }
    public override void Damage(float damage)
    {
        if (regen != null) { StopCoroutine(regen); }
        regen = regenarate(regenWait);
        StartCoroutine(regen);
        base.Damage(damage);
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
        StartCoroutine(DamageWithInvincible(0.15f, 3.0f));
    }
}

