using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMov : MonoBehaviour
{
    [Header("H Movement")]
    public float movSpeed = 5f;
    public Vector2 dir;
    public bool facingRight = true;

    [Header("V Movement")]
    public float jumpSpeed = 15f;
    public float jumpDelay = 0.25f;
    public float jumpTimer;

    [Header("Components")]
    public Rigidbody2D rb;
    public LayerMask groundLayer;
    public GameObject characterHolder;

    [SerializeField]
    public static bool playerControlsEnabled = true;

    [Header("Physics")]
    public float maxSpeed = 7f;
    public float linearDrag = 4f;
    public float gravity = 1;
    public float fallMultiplier = 5f;

    [Header("Collision")]
    public bool onGround = false;
    public float groundLength = 0.4f;
    public Vector3 colliderOffset;

    [Header("Dash")]
    public float dashDistance = 15f;
    bool isDashing;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bool wasOnGround = onGround;
        // Checks if on ground
        onGround = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, groundLength, groundLayer) || Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, groundLength, groundLayer);

        // When you land on ground squeezes player
        if (!wasOnGround && onGround)
        {
            StartCoroutine(JumpSqueeze(1.25f, 0.85f, 0.04f));
            //FindObjectOfType<AudioManager>().Play("Land");
        }

        if (Input.GetButtonDown("Jump") && playerControlsEnabled)
        {
            jumpTimer = Time.deltaTime + jumpDelay;
        }

        if (Input.GetKeyDown(KeyCode.S) && !isDashing && GameObject.Find("Wings") == null && playerControlsEnabled)
        {
            //FindObjectOfType<AudioManager>().Play("Dash");
            Dash();
        }

        // Directional input
        dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    void FixedUpdate()
    {
        if (playerControlsEnabled)
        {


            if (!isDashing)
            {
                CharacterMov(dir.x);
                PhysicsMod();
            }

            if (jumpTimer > Time.deltaTime && onGround)
            {
                Jump();
            }
        }
        else rb.velocity = Vector2.zero;
    }

    void CharacterMov(float horizontal)
    {
        // Moves char
        rb.AddForce(Vector2.right * horizontal * movSpeed);

        // Caps at max speed
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
        {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        }

        // Flips character
        if ((horizontal > 0 && !facingRight) || (horizontal < 0 && facingRight))
        {
            Flip();
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
        //FindObjectOfType<AudioManager>().Play("Jump");
        jumpTimer = 0;

        // Squeezes when jump
        StartCoroutine(JumpSqueeze(0.7f, 1.2f, 0.1f));
    }

    void Dash()
    {
        if (facingRight)
        {
            StartCoroutine(Dashing(1f));
        }
        else StartCoroutine(Dashing(-1f));
    }

    void PhysicsMod()
    {
        bool changDir = (dir.x > 0 && rb.velocity.x < 0) || (dir.x < 0 && rb.velocity.x > 0);

        // Changes characters linear drag to a higher value when player stops moving or while changing direction, 
        // making the player go slower through the floor
        if (onGround)
        {
            if (Mathf.Abs(dir.x) < 0.4f || changDir)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0;
            }

            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }

    public IEnumerator JumpSqueeze(float xSqueeze, float ySqueeze, float secs)
    {
        Vector3 originalSize = Vector3.one;
        Vector3 newSize = new Vector3(xSqueeze, ySqueeze, originalSize.z);

        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / secs;
            characterHolder.transform.localScale = Vector3.Lerp(originalSize, newSize, t);
            yield return null;
        }

        t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / secs;
            characterHolder.transform.localScale = Vector3.Lerp(newSize, originalSize, t);
            yield return null;
        }
    }

    public IEnumerator Dashing(float direction)
    {
        isDashing = true;
        rb.velocity = new Vector2(rb.velocity.x, 0f);
        rb.AddForce(new Vector2(dashDistance * direction, 0f), ForceMode2D.Impulse);

        float gravity = rb.gravityScale;
        rb.gravityScale = 0f;

        yield return new WaitForSeconds(0.4f);
        isDashing = false;
        rb.gravityScale = gravity;
    }

    public void Flip()
    {
        facingRight = !facingRight;
        transform.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * groundLength);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * groundLength);
    }
}
