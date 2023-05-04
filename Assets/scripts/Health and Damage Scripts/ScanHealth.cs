using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScanHealth : Health
{
    [SerializeField] ScanbotAI ActiveAi;
    AudioSource audioHurt;

    private void Start()
    {
        audioHurt = GetComponent<AudioSource>();
    }

    public override void Damage(float damage, float iFrames = 0.0f)
    {
        Debug.Log("We working?");
        audioHurt.Play();
        /*
        Debug.Log(ActiveAi.isElectric);
        if (ActiveAi.isElectric)
        {
            //Debug.Log("We working?");
            ActiveAi.dissapate();
            StartCoroutine(ActiveAi.Stun(1.0f));
        }
        */
        
        
            CurrHealth -= damage;
            if (CurrHealth <= 0)
                Die();
        
    }
}
