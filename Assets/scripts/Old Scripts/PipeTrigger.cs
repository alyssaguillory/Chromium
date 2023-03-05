using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeTrigger : MonoBehaviour
{
    public Rigidbody2D rb2D;
    public Animator animator; 
    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
        //Debug.Log("Game started"); 
    }

    // Update is called once per frame
    void Update()
    {
        //animator.ResetTrigger("playerIsNear");
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
   {
        if (collision.tag == "Player"){
            Debug.Log("Player seen");
            animator.SetTrigger("playerIsNear");  

        }else {
            animator.ResetTrigger("playerIsNear");
        }
        
        

   }
}
