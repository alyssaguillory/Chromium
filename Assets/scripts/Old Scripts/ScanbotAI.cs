using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;


public class ScanbotAI : MonoBehaviour
{
    [SerializeField] AudioSource AudioS;
    [SerializeField] AudioClip attackSound;

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
    public GameObject player;

    [Header("Movement")]
    public Transform[] waypoints; //patrol points
    [SerializeField] private float speed;
    private int waypointIndex; //current selected waypoint
    public float startWaitTime; //amount of time to pause at each waypoint
    private float waitTime;
    private Vector2 targetPos; //target position when moving away from player
    private float dist; //distance between player and scanbot
    public int direction; //set direction to move away from player

    private void Start()
    {
        anim = GetComponent<Animator>();
        //enemyPatrol = GetComponentInParent<EnemyPatrol>();

        rb = GetComponent<Rigidbody2D>();
        waypointIndex = 0;
        waitTime = startWaitTime;

    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;
        //transform.position = startPos.position;
        //Attack only when player in sight
        if (PlayerInSight())
        {
            if (cooldownTimer > attackCooldown)
            {
                cooldownTimer = 0;
                //anim.SetTrigger("rangedAttack");
                Debug.Log("player seen");
                Instantiate(ProjectilePrefab, LaunchOffset.position, Quaternion.identity);
                AudioS.PlayOneShot(attackSound);
            }

            switch (direction)
            {
                case 1: //right
                    targetPos = new Vector2(transform.position.x + 5, transform.position.y);
                    break;
                case 2: //left
                    targetPos = new Vector2(transform.position.x - 5, transform.position.y);
                    break;
                case 3: //up
                    targetPos = new Vector2(transform.position.x, transform.position.y + 5);
                    break;
                case 4: //down
                    targetPos = new Vector2(transform.position.x, transform.position.y - 5);
                    break;
            }
            
            //move away from player if less than 20 units away
            dist = Vector2.Distance(transform.position, player.transform.position);
            Debug.Log(dist);
            if(dist < 20f)
            {
                transform.position = Vector2.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            }
            
            cooldownTimer += Time.deltaTime;
        }
        else
        {
            if(waypoints.Length > 0)
            {
                // get distance between waypoint and scanbot
                dist = Vector2.Distance(transform.position, waypoints[waypointIndex].position);

                //if scanbot is within certain range, increase index to next waypoint
                if (dist < 2f)
                {
                    if (waitTime <= 0)
                    {
                        IncreaseIndex();
                        waitTime = startWaitTime;
                    }
                    else
                    {
                        waitTime -= Time.deltaTime;
                    }
                }
                Patrol();
            }  
        }
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

    private void Patrol()
    {
        transform.position = Vector2.MoveTowards(transform.position, waypoints[waypointIndex].position, speed * Time.deltaTime);
    }

    void IncreaseIndex()
    {
        waypointIndex++;
        if(waypointIndex >= waypoints.Length)
        {
            waypointIndex = 0;
        }
    }
}