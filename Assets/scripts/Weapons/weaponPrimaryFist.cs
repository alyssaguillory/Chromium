using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponPrimaryFist : WeaponBase
{
    private bool isActive = true;
    public Animator animator;
    public Transform fist;
    public LayerMask enemyLayers;
    public Vector2 start;
    public float attackRange = 10f;
    public float throwSpeed = 10.0f;
    public float damagingRange = 0.5f;
    public bool returning;
    private LineRenderer lr;
    public GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        start = fist.localPosition;
        lr = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            RenderLine();
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            //Vector2 toHitSpot = (-(Vector2)transform.position + mousePosition).normalized * 3;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetTrigger("Attack");
                //Attack();
            }
            if (returning) {
                if ((Vector2)transform.localPosition != start)
                {
                    transform.localPosition = Vector2.MoveTowards((Vector2)transform.localPosition, start, throwSpeed * Time.deltaTime);
                    DamageCheck();
                } else { returning = false; }
            } else if (Input.GetKey(KeyCode.Mouse0)) {
                if((start - (Vector2)transform.localPosition).magnitude < attackRange && ((Vector2)transform.position-mousePosition).magnitude > 0.2)
                {
                    transform.position = Vector2.MoveTowards((Vector2)transform.position, mousePosition, throwSpeed * Time.deltaTime);
                    DamageCheck();
                }
               else {
                    returning = true;
                    DamageCheck();
                } 

            } else if (Input.GetKeyUp(KeyCode.Mouse0)){
                returning = true;
            }
        }
    }
    void RenderLine()
    {
        lr.SetPosition(0, player.transform.position);
        lr.SetPosition(1, transform.position);
    }

    public override void Deactivate()
    {
        isActive = false;
        fist.GetComponent<SpriteRenderer>().enabled = false;
        //buzzSaw.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void Activate()
    {
        isActive = true;
        fist.GetComponent<SpriteRenderer>().enabled = true;
        //buzzSaw.GetComponent<SpriteRenderer>().enabled = true;
    }
    public void DamageCheck()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(fist.position, damagingRange, enemyLayers);
        if(hitEnemies.Length> 0) { returning = true; }
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<Enemy>() != null)
            {
                enemy.GetComponent<Enemy>().TakeDamage(1);
            }
            if (enemy.GetComponent<Health>() != null && enemy.gameObject.tag != "Player")
            {
                //Debug.Log("Enemy has been hit!");
                if (enemy.GetComponent<Health>().GetIframes() <= 0)
                {
                    //enemy.GetComponent<Health>().Damage(1);
                    StartCoroutine(enemy.GetComponent<Health>().DamageWithInvincible(0.5f, 1.0f));
                    if (enemy.gameObject.GetComponent<Rigidbody2D>() != null)
                    {
                        ThrowObject(enemy.gameObject.GetComponent<Rigidbody2D>(), fist, 100f);
                    }
                    //sawBlade.GetChild(1).position = enemy.transform.position;
                    //sawBlade.GetChild(1).LookAt(sawBlade);
                    //sawBlade.GetComponentInChildren<ParticleSystem>().Play();

                }
            }
        }
    }

    void ThrowObject(Rigidbody2D thrown, Transform thrower, float multiplier = 300f)
    {
        Vector2 RecoilVector = thrown.gameObject.GetComponent<Transform>().position - thrower.position;
        RecoilVector = RecoilVector.normalized * multiplier;
        if (RecoilVector.y < 0) { RecoilVector = new Vector2(RecoilVector.x, RecoilVector.y * -2); }
        thrown.AddForce(RecoilVector);
    }

}
