using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHealth : Health
{
    [SerializeField] MouseAi ActiveAi;
    public override void Damage(float damage)
    {
        Debug.Log("We working?");
        Debug.Log(ActiveAi.isElectric);
        if (ActiveAi.isElectric)
        {
            //Debug.Log("We working?");
            ActiveAi.dissapate();
            StartCoroutine(ActiveAi.Stun(1.0f));
        } else {
            CurrHealth -= damage;
            if (CurrHealth <= 0)
                Die();
        }
    }
    
}
