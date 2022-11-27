using UnityEngine;

public class ScanBotProjectile : MonoBehaviour
{
    public float Speed = 50;
  
    //public Vector2 targetDistance;
    
   

    public void Update()
    {
    
        //playerHealth = 
        transform.position += transform.right * Time.deltaTime * Speed;
        
        //targetDistance = target.position - rb.position;
        //transform.position += transform.right * targetDistance;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        Destroy(gameObject);
    }
}
