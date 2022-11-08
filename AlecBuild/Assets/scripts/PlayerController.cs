using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb2D;
    public Animator animator;
    public bool spacebar;

    public float speed;
    public float jumpForce;
    private float moveInput;
    public bool isMoving;
    public bool isGrounded;
    public bool isFighting;
    public Transform feetPos;
    public float checkRadius;
    public LayerMask whatIsGround;
    private float jumpUp = 0.0f;

    public float jumpTimeCounter;
    public float jumpTime;
    private bool isJumping;

    AudioSource audioSrc;


    // Start is called before the first frame update
    void Start()
    {
        //rb2D = gameObject.GetComponent<Rigidbody2D>();

        //moveSpeed = 3f;
        //jumpForce = 60f;
        //isJumping = false;
        rb2D = GetComponent<Rigidbody2D>();
        audioSrc = GetComponent<AudioSource>();
        //isFighting = false;
        
    }

    // Update is called once per frame
    
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Quit();
        }
        /*
        moveInput = Input.GetAxisRaw("Horizontal");
        isJumping = Input.GetButtonDown("Jump");
        isFighting = Input.GetKey(KeyCode.Mouse0);
        */
        
        if (moveInput > 0)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
            isMoving = true;
        }
        else if (moveInput < 0)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        if (isGrounded == true && jumpUp == 1.0f)
        {
            Jump();
            isJumping = true;
            jumpTimeCounter = jumpTime;
            isGrounded = false;
            rb2D.velocity += new Vector2(0.0f, jumpForce);
            //animator.SetBool("isJumping",true);
            //rb2D.velocity = Vector2.up * jumpForce;
        }
        if (jumpUp == 1.0f && isJumping == true)
        {
            if (jumpTimeCounter > 0)
            {
                isGrounded = false;
                
                //Jump();
                animator.SetBool("isJumping",true);
                jumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                isJumping = false;
            }

        }

    }
    void Update(){
        isGrounded = Physics2D.OverlapCircle(feetPos.position,checkRadius, whatIsGround);
        if(isGrounded && !isJumping)
        {
            animator.SetBool("isJumping", false);
            animator.ResetTrigger("Jump");
        }
        animator.SetBool("isFighting",false);
        //animator.SetBool("isJumping",false);
        moveInput = Input.GetAxisRaw("Horizontal");
        rb2D.velocity = new Vector2(moveInput * speed, rb2D.velocity.y);
        animator.SetFloat("Speed",Mathf.Abs(moveInput));

        spacebar = Input.GetButtonDown("Jump");


        if (Input.GetButtonDown("Jump")){
            isJumping = false;
            //animator.SetBool("isJumping",true);
            jumpUp = 1.0f;
        } if(Input.GetButtonUp("Jump"))
        {
            jumpUp = 0.0f;
        }
        if(Input.GetKey(KeyCode.Mouse0)){
            animator.SetBool("isFighting",true);
            
        } else {
            animator.SetBool("isFighting",false);
            
        }
        if (rb2D.velocity.x != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        if (isMoving)
        {
            if (!audioSrc.isPlaying)
            audioSrc.Play();
        }
        else
        {
            audioSrc.Stop();
        }
    }
    void Jump()
    {
        animator.SetTrigger("Jump");
    }
    void Quit()
    {
        Application.Quit();
    }
    //Will activate when object is selected, for debuggin
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 1, 0, 0.75F);
        Gizmos.DrawSphere(feetPos.position, checkRadius);
    }


}