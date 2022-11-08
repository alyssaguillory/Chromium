using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    AudioSource audioSrc;
    private AudioClip saw;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int attackDamage = 40;
    public bool isAttacking;

    void Start()
    {
        audioSrc = GetComponent<AudioSource>();
    }
    
    // Update is called once per frame
    void Update()
    {   
        
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            Attack();
        }
        
        
    }
    void Attack()
    {
        SoundManager.instance.PlaySound(saw);
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("Enemy has been hit!");
            enemy.GetComponent<Enemy>().TakeDamage(1);
            
        }
    }
    void OnDrawGizmoSelected()
    {
        if(attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);

    }
    
    
}
