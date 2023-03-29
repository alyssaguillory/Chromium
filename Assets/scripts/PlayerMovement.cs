using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //These are the animation curves used for acceleration
    [SerializeField] AnimationCurve horizontalForce;
    [SerializeField] AnimationCurve jumpForce;
    private IEnumerator jump;

    //These two objects determine the Rigidbody used and the Animator Used
    private Rigidbody2D rb2D;
    [SerializeField] private Animator animator;

    //These Determine the forces used
    [SerializeField] private float speed = 50.0f;
    [SerializeField] private float jumpMultiplier = 100.0f;
    [SerializeField] private float jumpUp = 0.0f;

    //These check what the player is doing
    public bool isGrounded;
    public bool isFighting;

    //These are used for data checking
    [SerializeField] private Transform feetPos;
    [SerializeField] private float checkRadius;
    [SerializeField] private LayerMask whatIsGround;

    //This is to ensure that you cannot jump super hight
    [SerializeField] private float jumpTimeCounter;
    [SerializeField] private float jumpTime;
    private bool isJumping;

    //This is used to cap max speed
    [SerializeField] private float MaxSpeed = 10.0f;
    private Vector3 respawnPoint;

    //This is used to get the movement curves
    private float horizonTime;
    private float vertTime = 0.0f;

    //These are not used
    public bool spacebar;
    //public Transform buzzSaw;
    private float horizonMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        //this is where we get the different components, and need to initialize new components we might use
        rb2D = GetComponent<Rigidbody2D>();

    }

    // Fixed Update is called per physics frame and should be used for physics
    void FixedUpdate()
    {
        //The force actual used to push the character in the left and right direction will change depending on how long you are holding the movement key down
        Vector2 force = new Vector2(horizontalForce.Evaluate(horizonTime) * horizonMultiplier * speed, jumpForce.Evaluate(vertTime) * jumpMultiplier);
        rb2D.AddForce(force); //Adds force
        if (rb2D.velocity.x > MaxSpeed) { 
            rb2D.velocity = new Vector2(MaxSpeed, rb2D.velocity.y);
            //Check to see if the resulting velocity is higher than maxVelocity, it it is, then start lowering velocity
        } else if (rb2D.velocity.x < -MaxSpeed) { rb2D.velocity = new Vector2(-MaxSpeed, rb2D.velocity.y); }
        
        
        //Check if the bot is on the ground and we are trying to jump. If so, then set us to jump.
        
    }

    // Update is called once per frame
    void Update()
    {
        //Check Grounds
        //if (!Input.GetKey(KeyCode.Space)) { isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround); }
        if (Input.GetKey(KeyCode.Space)){
            if (Input.GetKeyDown(KeyCode.Space) && !isGrounded) { vertTime = 1; }
            if (isGrounded) { vertTime = 0; isGrounded = false; }
            vertTime = vertTime + Time.deltaTime*2;
            if(vertTime > 0.5f) { isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround); }
            Debug.Log(vertTime);
            //Debug.Log(jumpForce.Evaluate(vertTime));
        } else if(Input.GetKeyUp(KeyCode.Space)){ vertTime = 0; } else { isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround); }

        //Horizontal Movement
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) {
            //SetMultipliers and reset time
            if (Input.GetKeyDown(KeyCode.A)) { horizonMultiplier = -1; } 
            else { horizonMultiplier = 1; }
            horizonTime = 0;
        } else if ((Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))) {
            //Make sure that the bugs get ironed out
            //If you release one and are still holding a button, you can change direction
            if (Input.GetKey(KeyCode.A)) {
                if(horizonMultiplier != -1) {
                    horizonMultiplier = -1;
                    horizonTime = 0;
                }
            } else if (Input.GetKey(KeyCode.D)) {
                if(horizonMultiplier != 1) {
                    horizonMultiplier = 1;
                    horizonTime = 0;
                }
            } else { horizonTime = 0; horizonMultiplier = 0; } // Else it stops you    
        } else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)) { horizonTime++; } //Increases timer as long as you are trying to move

        //Need Animation
        /*
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         * 
         */
    }

    private void OnDrawGizmosSelected()
    {
        //Will activate when object is selected, for debugging. Shows you the circle that is being checked for is grounded
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawSphere(feetPos.position, checkRadius);
    }


    //Jump Coroutine
    //Activate countdown
    //while holding space add force and reduce the force pool
    //When releasing space, slow down quickly and dont allow pressing it again.
    //Once cooldown is done, start checking for grounding again

}
