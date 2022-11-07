using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //-------------------walking-------------------
    public float walkSpeed = 15;

    //-------------------jumping-------------------
    public float jumpForce = 30;
    public float fallMultiplier = 2.5f;
	public float lowJumpMultiplier = 2f;
    public Transform groundCheck;
    public LayerMask ground;

    //-------------------Misc-------------------

    public LayerMask platformLayer;

    private float horizontalInput;
    private float verticalInput;

    private bool grounded;

    //-------------------Components-------------------
    private Rigidbody2D rb;
    //private Animator anim;
    

    void Start()
    {
        //-------------------assigning components-------------------
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
    }


    void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0f, ground);
        
        //-------------------walking-------------------
        if (grounded || horizontalInput != 0) 
            rb.velocity = new Vector2(horizontalInput*walkSpeed, rb.velocity.y);

        //-------------------jump-------------------
        if (Input.GetButtonDown("Jump") && grounded) 
            rb.velocity += Vector2.up*jumpForce;
        
        if(rb.velocity.y < 0) //better jump from https://youtu.be/7KiK0Aqtmzc
			rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
			rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;

        //-------------------fall through platform-------------------
        
        Collider2D platform = Physics2D.OverlapCircle(groundCheck.position, .1f, platformLayer);
        if(platform != null && Input.GetAxisRaw("Vertical") < 0){
            StartCoroutine("FallThroughPlatform", platform.GetComponent<PlatformEffector2D>());
        }
/*
        //-------------------ANIMAÇÃO-------------------
        if(grounded){
            if(Mathf.Abs(rb.velocity.x) > 0)
                anim.Play("PlayerWalk");
            else
                anim.Play("PlayerIdle");
        } else{
            if (rb.velocity.y > 5f)
                anim.Play("PlayerJumpRise");
            else if (rb.velocity.y > -5f)
                anim.Play("PlayerJumpPeak");
            else
                anim.Play("PlayerJumpFall");
        }*/

    }

    IEnumerator FallThroughPlatform(PlatformEffector2D platform)
    {
        platform.rotationalOffset = 180f;
        yield return new WaitForSeconds (.5f);
        platform.rotationalOffset = 0f;
    }
}
