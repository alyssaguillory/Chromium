using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    [SerializeField] AudioSource AudioS;
    [SerializeField] AudioClip healthUp;
    // Start is called before the first frame update
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerHealth victim = collision.attachedRigidbody.gameObject.GetComponent<PlayerHealth>();
        if (victim != null)
        {
            victim.AddBar();
            AudioS.PlayOneShot(healthUp);
            gameObject.transform.position = gameObject.transform.position * 1000;
        }
    }
}
