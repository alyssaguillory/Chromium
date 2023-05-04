using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHealth : Health
{
    [SerializeField] MouseAi ActiveAi;
    [SerializeField] AudioClip dissapteDamage;
    

    private void Start()
    {
        
    }
    
    public override void Damage(float damage, float iFrames = 0.0f)
    {
        if (ActiveAi.isElectric)
        {
            PlaySound(dissapteDamage);
            ActiveAi.dissapate();
            StartCoroutine(ActiveAi.Stun(1.0f));
            StartCoroutine(IFrameActivator(0.5f));
        } else {
            base.Damage(damage, iFrames);
        }
    }
    
}
