using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    
    [Header("Walking")]
    //-------------------walking-------------------
    public float walkSpeed = 15;
    public float walkAccel = 45;
    public float airAccel = 25;
    public float groundedDecel = 30;

    [Header("Jumping")]
    //-------------------jumping-------------------
    public float jumpForce = 30;
    public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
    public Transform groundCheck;
    public LayerMask ground;

    //-------------------Misc-------------------
    private Vector2 moveInput;
    [HideInInspector]
    public bool grounded;
    public bool facingRight;

    //-------------------Components-------------------
    private Rigidbody2D rb;
    private Animator anim;
    

    void Start()
    {
        //-------------------assigning components-------------------
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        // friction bug fix
    }


    void Update()
    {
        moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0f, ground);
        
        //-------------------walking-------------------

        if(moveInput.x != 0){
            if(grounded){
                Accelerate(walkAccel);
            } else{
                Accelerate(airAccel);
            }
        }else if(grounded){
            float velocityToAdd = -rb.velocity.normalized.x * groundedDecel * Time.deltaTime;
            if(Mathf.Abs(rb.velocity.magnitude) > .01f){
                velocityToAdd = Mathf.Clamp(velocityToAdd, -rb.velocity.magnitude, rb.velocity.magnitude);
                rb.AddForce(velocityToAdd * Vector2.right, ForceMode2D.Impulse);
            } else{
                rb.velocity = Vector2.zero;
            }
        }

        if (moveInput.x > 0 && !facingRight)
            Flip ();
        else if (moveInput.x < 0 && facingRight) 
            Flip ();

        //-------------------jump-------------------
        if (Input.GetButtonDown("Jump") && grounded) 
            rb.AddForce(Vector2.up*jumpForce, ForceMode2D.Impulse);
        
        if(rb.velocity.y < 0) //better jump from https://youtu.be/7KiK0Aqtmzc
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        //-------------------fall through platform-------------------

        //-------------------ANIMAÇÃO-------------------
        if(grounded){
            if(Mathf.Abs(rb.velocity.x) > 0)
                anim.Play("Walk");
            else
                anim.Play("Idle");
        } else{
            if (rb.velocity.y > 5f)
                anim.Play("Jump");
            else if (rb.velocity.y > -5f)
                anim.Play("FallStart");
            else
                anim.Play("Fall");
        }

    }

    void Accelerate(float acceleration){
        float velocityToAdd = moveInput.x * acceleration * Time.deltaTime;
        if(Mathf.Abs(rb.velocity.x + velocityToAdd) <= walkSpeed || Mathf.Abs(rb.velocity.x + velocityToAdd) < Mathf.Abs(rb.velocity.x)){
            rb.AddForce(velocityToAdd * Vector2.right, ForceMode2D.Impulse);
        }
    }

    public void setMaxSpeed(float walkSpeed){
        this.walkSpeed = walkSpeed;
    }
    
    public void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }
}
