using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public Animator animator;
    public bool spacebar;
   

    public float speed;
    private float originalSpeed; //original speed before slowed down
    private float minSpeed;
    public float jumpForce;
    private float moveInput;
    public bool isGrounded;
    public bool isFighting;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    public Transform buzzSaw;
    private float jumpUp = 0.0f;

    public float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;
    public float MaxSpeed = 10.0f;
    private Vector3 respawnPoint;


    // Start is called before the first frame update
    void Start()
    {
        //this is where we get the different components, and need to initialize new components we might use
        rb2D = GetComponent<Rigidbody2D>();
        
        //used for mouse slowing down player
        originalSpeed = speed;
        minSpeed = originalSpeed / 3f;
        
    }

    // Fixed Update is called per physics frame and should be used for physics
    
    void FixedUpdate()
    {
        /*
        moveInput = Input.GetAxisRaw("Horizontal");
        isJumping = Input.GetButtonDown("Jump");
        isFighting = Input.GetKey(KeyCode.Mouse0);
        */
        //Make player move slower when not grounded

        //Mirros the positions of the robot
        //if (moveInput > 0) {transform.eulerAngles = new Vector3(0, 0, 0);}
        //else if (moveInput < 0) {transform.eulerAngles = new Vector3(0, 180, 0); buzzSaw.transform.eulerAngles = new Vector3(0, 180, 0); }
        //Adds to the velocity of the robot
        Vector2 force = new Vector2(moveInput * speed, 0);
        if (!isGrounded) { force = force * 0.5f; }
        rb2D.AddForce(force);
        if(rb2D.velocity.x > MaxSpeed) { rb2D.velocity = new Vector2(MaxSpeed, rb2D.velocity.y); }
        else if (rb2D.velocity.x < -MaxSpeed) { rb2D.velocity = new Vector2(-MaxSpeed, rb2D.velocity.y); }
        //Impulse
        //How do I cap the velocity
        //Applying forces?
        //Check if the bot is on the ground and we are trying to jump. If so, then set us to jump.
        
        
        if (isGrounded == true && Input.GetKey(KeyCode.Space)){
            //Used for animation, need fixed
            Jump();
            //Used because I can't figure it out without it
            isJumping = true;

            //Used to determin if still jumping
            jumpTimeCounter = jumpTime;
            isGrounded = false;
            //Used to start the upwards jump
            rb2D.velocity += new Vector2(0.0f, jumpForce);
        }
        
        //While they are holding space down
        if (jumpUp == 1.0f && isJumping == true) {
            //if airtime has not run out
            if (jumpTimeCounter > 0){
                //Make sure isGrounded is false, need fixed as it causes bunny hops. If not here, then it prevents
                //you from jumping again
                isGrounded = false;
                //Decrement airtime
                jumpTimeCounter -= Time.deltaTime;
                //Used for animation
                animator.SetBool("isJumping",true);  
            } else {
                isJumping = false;
            }
        }
    }

    //Update is per frame and is where we fetch inputs and do non-physics stuff
    void Update(){
        //Check Grounds
        if(!Input.GetKey(KeyCode.Space)) { isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround); }
        
        //Animation Stuff
        if(isGrounded && !isJumping) {
            animator.SetBool("isJumping", false);
            animator.ResetTrigger("Jump");
        }
        animator.SetBool("isFighting",false);
        //animator.SetBool("isJumping",false);
        moveInput = Input.GetAxisRaw("Horizontal");
        //Animation
        animator.SetFloat("Speed",Mathf.Abs(moveInput));

        //See if you are pressing space
        if (Input.GetButtonDown("Jump")) {
            isJumping = false;
            jumpUp = 1.0f;
        } else if(Input.GetButtonUp("Jump")) { //If you release
            jumpUp = 0.0f;
        }
        //Upon left clicking start fighting. Mostly for animation
        if(Input.GetKey(KeyCode.Mouse0)){
            animator.SetBool("isFighting",true);
            //gets mouse input for dynamic attack position **unused
            //Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        } else {
            animator.SetBool("isFighting",false);
        }
    }
    void Jump() //Animation witchcraft
    {
        animator.SetTrigger("Jump");
    }
    //Will activate when object is selected, for debugging. Shows you the circle that is being checked for is
    //grounded
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawSphere(feetPos.position, checkRadius);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "fallCollider"){
            transform.position = respawnPoint;
           
            
        } else if (collision.tag == "Checkpoint"){
            respawnPoint = transform.position;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Steam")
        {
            StartCoroutine(gameObject.GetComponent<Health>().DamageWithInvincible(0.5f, 20.0f));
        }
    }

    public virtual IEnumerator SlowedByMouse()
    {
        speed = 0;
        yield return new WaitForSeconds(.25f);
        speed = originalSpeed / 3f;

        yield return new WaitForSeconds(.10f);
        speed = originalSpeed / 2f;

        yield return new WaitForSeconds(.10f);
        speed = originalSpeed / 1.5f;

        yield return new WaitForSeconds(.10f);
        speed = originalSpeed;

    }


}