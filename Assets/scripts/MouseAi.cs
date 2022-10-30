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

    new public SpriteRenderer[] ears;

    bool isStunned = false;
    bool active = false;
    float stunDur = 0.0f;
    bool isGrounded = false;
    public bool Dead = false;
    
    public float jumpForce = 100.0f;
    public float safeSpace = 1.0f;
    public float activeArea = 5.0f;
    public float checkRadius = 1.0f;
    public LayerMask whatIsGround;


    void Start()
    {
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
        //When the player gets into the active range of the mouse, it will activate.
        Vector2 targetDistance = (Vector2)target.position - (Vector2)rb.position;
        if (!active && targetDistance.magnitude < activeArea && !Dead)
        {
            active = true;
            InvokeRepeating("UpdatePath", 0f, 0.5f);
            //Activate the sprite renderer
            foreach (SpriteRenderer a in ears)
            {
                a.enabled = true;
            }
            //Do particle effects
        }
        isGrounded = Physics2D.OverlapCircle(rb.position, checkRadius, whatIsGround);
        //Have the mouse look in the direction of the player
        if(targetDistance.x > 0 && !Dead) { transform.eulerAngles = new Vector2(0, 180); } else if (targetDistance.x < 0) { transform.eulerAngles = new Vector2(0, 0); }
    }

    // Update is called once per frame
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
        Vector2 targetDistance = (Vector2)target.position - (Vector2)rb.position;
        //if the mouse is near the jump zone, have it jump
        if (targetDistance.magnitude < safeSpace && isGrounded && active && !Dead)
        {
            //else have it continue to path
            Vector2 force = Vector2.up * jumpForce;
            rb.AddForce(force);

        } else if (active && isGrounded) { FollowPath(); }
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
            Gizmos.DrawSphere(rb.position, activeArea);
            Gizmos.DrawSphere(target.position, safeSpace);
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