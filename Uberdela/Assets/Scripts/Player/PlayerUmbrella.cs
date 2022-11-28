using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUmbrella : MonoBehaviour
{
    public float clampedSpeed = 3f; // max speed when moving against umbrella
    public float clampingCurve = 250f; // max speed when moving against umbrella
    public float gravitySupression = .5f;
    [HideInInspector]
    public Vector3 mousePos;
    private float mouseAngle;
    private bool facingRight = true;
    [HideInInspector]
    public bool active = true;
    private float originalJump;
    private float originalGravity;

    //private bool clamping = false;
    // Component References
    public Transform player;
    private PlayerMovement playerMovement;
    private Rigidbody2D playerRB;
    private Animator anim;

    void Start()
    {
        playerMovement = player.gameObject.GetComponent<PlayerMovement>();
        playerRB = player.gameObject.GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();

        originalJump = playerMovement.jumpForce;
        originalGravity = playerRB.gravityScale;
    }

    void LateUpdate()
    {
        mousePos= Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10;
        mouseAngle = Vector3.Angle (Vector3.down, mousePos - transform.position);
        //clamping ^= Input.GetButtonDown("Fire1");


        if(Input.GetButton("Fire1") && !Input.GetButtonDown("Jump")){
            playerRB.gravityScale = originalGravity / gravitySupression;
            playerMovement.jumpForce = originalJump / gravitySupression;
            AnimateRotation();
            if(active)
                ClampVelocity();
        }else{
            playerRB.gravityScale = originalGravity;
            playerMovement.jumpForce = originalJump;
            anim.Play("down");
        }
    }

    void AnimateRotation(){

        int treatedAngle = (int)mouseAngle / 36;
        
        switch(treatedAngle){
        case 4: // pointing straight up
            anim.Play("up");
            break;
        case 3: // pointing 45 degrees up
            anim.Play("diagonalUp");
            break;
        case 2: // pointing straight forward
            anim.Play("straight");
            break;
        case 1: // pointing 45 degrees down
            anim.Play("diagonalDown");
            break;
        case 0: // pointing straight down
            anim.Play("down");
            break;
        default: // ¯\_(ツ)_/¯
            anim.Play("down");
            break;
        }

        if ((mousePos - player.position).x > 0 && !facingRight)
            playerMovement.Flip();
        else if ((mousePos - player.position).x < 0 && facingRight)
            playerMovement.Flip();
    }

    void ClampVelocity(){
        float clampingAngle = Vector2.Angle(playerRB.velocity, mousePos - transform.position) - 90;
        
        float clampedVelocity = (clampingCurve / clampingAngle) - (clampingCurve / 90) + clampedSpeed; //smoothly transition between not clamping velocity and clamping it at clampedSpeed

        Debug.DrawLine(transform.position, mousePos);
        Debug.DrawLine(transform.position, playerRB.velocity + (Vector2)transform.position);

        if(clampingAngle > 0){
            playerRB.velocity = Vector2.ClampMagnitude(playerRB.velocity, clampedVelocity);
        }
        
        /*
        // clamp velocity using line intersection
        Vector3 relativeMousePos = mousePos - transform.position;
        Vector3 clampline1 = RotateByRad(relativeMousePos, Mathf.PI/2).normalized * 999 - relativeMousePos.normalized * clampedSpeed;
        Vector3 clampline2 = RotateByRad(relativeMousePos, Mathf.PI/2).normalized * -999 - relativeMousePos.normalized * clampedSpeed;
        Vector3 newClamped;
        
        Debug.Log(LineLineIntersection(out newClamped, Vector3.zero, playerRB.velocity, clampline1/2, clampline2/2));
        Debug.DrawLine(clampline1 + transform.position, clampline2 + transform.position);
        Debug.DrawLine(transform.position, playerRB.velocity + (Vector2)transform.position);

        if(clampingAngle > 0)
            playerRB.velocity = Vector3.ClampMagnitude(playerRB.velocity, newClamped.magnitude);*/
    }

    Vector3 RotateByRad(Vector2 v, float ang){
        float sin = Mathf.Sin(ang);
        float cos = Mathf.Cos(ang);
        return new Vector2(cos*v.x - sin*v.y, sin*v.x + cos*v.y);
    }

    // ngl i just found this somewhere
    public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
            Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2){

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if( Mathf.Abs(planarFactor) < 0.0001f 
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) 
                    / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }
}
