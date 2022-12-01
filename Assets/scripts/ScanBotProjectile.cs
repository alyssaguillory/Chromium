using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ScanBotProjectile : MonoBehaviour
{
    public float Speed = 50;
  
    //public Vector2 targetDistance;
    
   public void Start() {
        
   }

    public void Update()
    {
        StartCoroutine(WaitBeforeShow());
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
        
        Destroy(gameObject);
    }
}
