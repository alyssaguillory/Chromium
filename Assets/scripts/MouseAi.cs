using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class MouseAi : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    public SpriteRenderer[] ears;

    bool isStunned = false;
    bool isElectric = false;
    bool active = false;
    public float stunDur = 0.0f;
    bool isGrounded = false;
    bool Dead = false;
    
    public float jumpForce = 100.0f;
    public float safeSpace = 1.0f;
    public float outerEdge = 2.5f;
    public float activateArea = 5.0f;
    public float checkRadius = 1.0f;
    public float Recharge = 2.5f;
    public LayerMask whatIsGround;
    public int State = 0;
    public Vector2 targetDistance;
    bool dealtDamage;
    float padTime;

    void Start()
    {
        //Initialize A* pathfinding, and get the components. This will also set the renderer to be disabled so you can only see the tail of the mouse
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        foreach (SpriteRenderer a in ears)
        {
            a.enabled = false;
        }
        

        //seeker.StartPath(rb.position, target.position, OnPathComplete);

    }
    private void Update()
    {
        //Check the distance to the target and make sure the mouse is grounded
        targetDistance = (Vector2)target.position - (Vector2)rb.position;
        isGrounded = Physics2D.OverlapCircle(rb.position, checkRadius, whatIsGround);
        //Call States
        if (State < 2)
        {
            States();
        }
        
        //Have the mouse look in the direction of the player
        if(targetDistance.x > 0 && !Dead) { transform.eulerAngles = new Vector2(0, 180); } else if (targetDistance.x < 0) { transform.eulerAngles = new Vector2(0, 0); }
    }

    // Fixed Update is called for physics frames
    void FixedUpdate()
    {
        if (path == null)
            return;
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }
        if (State >= 2)
        {
            targetDistance = (Vector2)target.position - (Vector2)rb.position;
            States();
        }
        
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void FollowPath()
    {
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
    
    void States()
    {
        switch (State)
        {
            case 0: //Hide
                Hidden();
                //while in state zero, we should be checking that Player is not within shpere of influence, if it is change state to 2 and play electric
                break;
            case 1: //Die
                Die();
                break;
            case 2: //Pursue
                Pursue();
                //while in state one, the mouse should rush towards the player and jump, when they get within range, they go to state 4
                break;
            case 3: //Stunned
                Stunned();
                break;
            case 4: //Attack
                Attack();
                //While in this state if has enough time left for electricity, then jump at the player. Else state = 5
                break;
            case 5: //Flee
                Flee();
                //
                break;
        }
    }

    void Hidden()
    {
        if (!active && targetDistance.magnitude < activateArea && !Dead)
        {
            active = true;
            InvokeRepeating("UpdatePath", 0f, 0.5f);
            //Activate the sprite renderer
            foreach (SpriteRenderer a in ears)
            {
                a.enabled = true;
            }
            State = 2;
            isElectric = true;
            //Do particle effects
        }
    }

    void Pursue()
    {
        if (path == null)
            return;
        FollowPath();
        
        //if the mouse is near the jump zone, have it jump
        if (targetDistance.magnitude < safeSpace && isGrounded && active && !Dead)
        {
            //else have it continue to path
            Vector2 force = Vector2.up * jumpForce;
            rb.AddForce(force);
            State = 4;
            padTime = 0.5f;
            Attack();
        }
        else if (!isElectric) { Flee(); }
        else if (active && isGrounded) { FollowPath(); }
        
    }

    void Stunned()
    {

    }

    void Attack()
    {
        if (isGrounded && padTime<0.5)
        {
            State = 5;
            Recharge = 2.5f;
            return;
        }
        if (rb.gameObject.GetComponent<Collider2D>().bounds.Intersects(target.gameObject.GetComponent<Collider2D>().bounds))
        {
            Debug.Log("Bounds intersecting");
            dealtDamage = true;
        }
        padTime -= Time.deltaTime;
    }

     void Flee()
    {
        if(Recharge > 0) { 
            Recharge -= Time.deltaTime;
            if(targetDistance.x > 0)
            {
                if (Mathf.Abs(targetDistance.x) > activateArea)
                {
                    rb.AddForce(Vector2.right * speed * 0.75f * Time.deltaTime);
                } else
                {
                    rb.AddForce(Vector2.left * speed * 0.75f * Time.deltaTime);
                }
            } else if(targetDistance.x < 0)
            {
                if (Mathf.Abs(targetDistance.x) < activateArea)
                {
                    rb.AddForce(Vector2.right * speed * 0.75f * Time.deltaTime);
                }
                else
                {
                    rb.AddForce(Vector2.left * speed * 0.75f * Time.deltaTime);
                }
            } 
        } else { State = 2; dealtDamage = false; }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        if (rb != null)
        {
            Gizmos.DrawSphere(rb.position, activateArea);
            Gizmos.DrawSphere(target.position, safeSpace);
            Gizmos.DrawSphere(rb.position, checkRadius);
        }
    }

    public void Die()
    {
        Dead = true;
    }
}
/*[x]The mouse will be inactive until the player gets close.
 *[x]While hidden, it is invisible except the tail.
 *[x]When it unhides, it immediately electrifies and rushes towards the player
 *[x]When it gets close, it will jump upwards towards the player [currently can get bugged if it lands in safe zone]
 *Upon connecting with the player, it will either hit the saw or the player
 *Upon hitting the saw, the mouse is stunned and knocked back
 *Upon hitting the player the player is damaged and knocked back, and the mouse is knocked back
 *Either way it looses its electrification
 *While unelectric The mouse will attempt to stay a distance away from the player and will move slower going backwards
 *When it electrifies, it will turn away, run a bit, and then charge again.
 */

/* States
 * 0 Hidden: Does not move, or just tail moves. When player comes within range, activates the mouse.
 * 1 Dead: sets dead to true, which on the enemy script replaces it with a corpse.
 * 2 Chasing: Quickly moves towards the character, will only chase if they are electrified
 * 3 Stunned: If they are damaged while electrified, they do not take damage, but are instead flung away and stunned/inactive for a few seconds
 * 4 Attacking: If they get within a certain range of the player and have time left on their electrifier, they will jump at the player at a high speed. Else they flee
 * 5 Fleeing: The mouse backs up at half the speed to try and keep in a certain radius around the player.
 */