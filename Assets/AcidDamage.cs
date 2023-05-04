using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcidDamage : MonoBehaviour
{
    [SerializeField] AudioSource AudioS;
    [SerializeField] AudioClip acidSound;
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        Health victim = collision.attachedRigidbody.gameObject.GetComponent<Health>();
        if(victim != null)
        {
            victim.Damage(1);
            AudioS.PlayOneShot(acidSound);
        }
    }
}
