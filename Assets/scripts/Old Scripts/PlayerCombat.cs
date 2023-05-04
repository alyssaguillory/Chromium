using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerCombat : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;
    private Rigidbody2D rb2D;
    private Vector2 start;

    public Transform attackPoint;
    public Transform sawBlade;
    public Transform buzzSaw;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;

    public int attackDamage = 40;
    [SerializeField] Vector2 RecoilForce = new Vector2(1, 0);
    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        start = sawBlade.localPosition;
    }


    // Update is called once per frame
    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 toHitSpot = (-rb2D.position + mousePosition).normalized*3;

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
            //sawBlade.gameObject.GetComponent<CircleCollider2D>().enabled = true;
            Attack();
        }
        else { animator.SetBool("isFighting", false); sawBlade.localPosition = (Vector2)sawBlade.localPosition - ((Vector2)sawBlade.localPosition - start) * Time.deltaTime * 2; /*sawBlade.gameObject.GetComponent<CircleCollider2D>().enabled = false; */}

    }
    void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(sawBlade.position,attackRange,enemyLayers);

        foreach(Collider2D enemy in hitEnemies)
        {
            if(enemy.GetComponent<Health>() != null && enemy.gameObject.tag != "Player")
            {
                enemy.GetComponent<Health>().Damage(1.0f, 0.5f);
            }
            if (enemy.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                Debug.Log("Throwing");
                ThrowObject(enemy.gameObject.GetComponent<Rigidbody2D>(), sawBlade, 3000f);
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
/*To find the location that the buzzsaw should go to find the point of the mouse in world cordinates
 * find teh vector to that direction
 * add a portion of that vector to the location
 * change the direction over time to it 
 */