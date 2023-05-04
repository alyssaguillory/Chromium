using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponPrimarySaw : WeaponBase
{
    private bool isActive = false;
    public Animator animator;
    public Transform sawBlade;
    public Transform buzzSaw;
    public LayerMask enemyLayers;
    private Vector2 start;
    public float attackRange = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        start = sawBlade.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 toHitSpot = (-(Vector2)transform.position + mousePosition).normalized * 3;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetTrigger("Attack");
                Attack();
            }
            if (Input.GetKey(KeyCode.Mouse0))
            {
                sawBlade.localPosition = sawBlade.localPosition + ((Vector3)toHitSpot + (Vector3)start - sawBlade.localPosition) * Time.deltaTime * 8;
                buzzSaw.Rotate(0.0f, 0.0f, 5.0f);
                animator.SetBool("isFighting", true);
                sawBlade.gameObject.GetComponent<CircleCollider2D>().enabled = true;
                Attack();
            }
            else { animator.SetBool("isFighting", false); sawBlade.localPosition = (Vector2)sawBlade.localPosition - ((Vector2)sawBlade.localPosition - start) * Time.deltaTime * 2; sawBlade.gameObject.GetComponent<CircleCollider2D>().enabled = false; }
        }
    }
    public override void Deactivate()
    {
        isActive = false;
        sawBlade.GetComponent<SpriteRenderer>().enabled = false;
        buzzSaw.GetComponent<SpriteRenderer>().enabled = false;
    }
    public override void Activate()
    {
        isActive = true;
        sawBlade.GetComponent<SpriteRenderer>().enabled = true;
        buzzSaw.GetComponent<SpriteRenderer>().enabled = true;
    }

    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(sawBlade.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            
            if (enemy.GetComponent<Health>() != null && enemy.gameObject.tag != "Player")
            {
                enemy.GetComponent<Health>().Damage(5.0f, 0.5f);
            }
            if (enemy.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                ThrowObject(enemy.gameObject.GetComponent<Rigidbody2D>(), sawBlade, 100f);
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawSphere(sawBlade.position, attackRange);
    }
    void ThrowObject(Rigidbody2D thrown, Transform thrower, float multiplier = 300f)
    {
        Vector2 RecoilVector = thrown.gameObject.GetComponent<Transform>().position - thrower.position;
        RecoilVector = RecoilVector.normalized * multiplier;
        if (RecoilVector.y < 0) { RecoilVector = new Vector2(RecoilVector.x, RecoilVector.y * -2); }
        thrown.AddForce(RecoilVector);
    }
}
