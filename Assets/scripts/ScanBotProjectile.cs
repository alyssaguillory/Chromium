using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ScanBotProjectile : MonoBehaviour
{
    public float Speed = 50;
    public GameObject player;
    Vector2 targetVector;
    //public Vector2 targetDistance;
    
   public void Start() {
        player = GameObject.FindGameObjectWithTag("Player");
        targetVector = (Vector2)(player.transform.position - transform.position).normalized;
        //transform.LookAt(player.transform, Vector3.up);
    }

    public void Update()
    {
        //StartCoroutine(WaitBeforeShow());
        transform.position += (Vector3)targetVector * Time.deltaTime * 40;
        //playerHealth = 
        //transform.position += transform.right * Time.deltaTime * Speed;
        
        //targetDistance = target.position - rb.position;
        //transform.position += transform.right * targetDistance;
        //StartCoroutine(WaitBeforeShow());
    }
    private IEnumerator WaitBeforeShow()
    {
           
           
           transform.position += transform.right * Time.deltaTime * Speed;
           transform.position += transform.right * Time.deltaTime * Speed*2;
           
           yield return new WaitForSeconds(5f);
    
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<Health>() != null)
            StartCoroutine(collision.gameObject.GetComponent<Health>().DamageWithInvincible(0.2f, 20.0f));
        Destroy(transform.parent.gameObject);
    }
}
