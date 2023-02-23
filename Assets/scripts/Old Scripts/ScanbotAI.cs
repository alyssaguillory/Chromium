using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class ScanbotAI : MonoBehaviour
{
    public GameObject ProjectilePrefab;
    public Transform LaunchOffset; 
    
    public Transform startPos; 
    Rigidbody2D rb;
    private Vector2 targetDistance; 

    [Header("Attack Parameter")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private float damage;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private CircleCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    private EnemyPatrol enemyPatrol;
    public Animator anim;
    public Health HealthController;
    public Transform target; 
    
  



    private void Start()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
         
        rb = GetComponent<Rigidbody2D>(); 

        
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        transform.position = startPos.position;
        //Attack only when player in sight
        if (PlayerInSight())
        {
            if (cooldownTimer > attackCooldown)
            {
                cooldownTimer = 0;
                //anim.SetTrigger("rangedAttack");
                transform.position = startPos.position; 
                Debug.Log("player seen");
                Instantiate(ProjectilePrefab, LaunchOffset.position, Quaternion.identity);
            }
            cooldownTimer += Time.deltaTime;
        }
        if (enemyPatrol != null)
        {
            enemyPatrol.enabled = !PlayerInSight();
        }
        targetDistance = (Vector2)target.position - (Vector2)rb.position;
        if(targetDistance.x > 0) { transform.eulerAngles = new Vector2(0, 180); } else if (targetDistance.x < 0) { transform.eulerAngles = new Vector2(0, 0); }
            
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y * range, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);


        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y * range, boxCollider.bounds.size.z));
    }
}