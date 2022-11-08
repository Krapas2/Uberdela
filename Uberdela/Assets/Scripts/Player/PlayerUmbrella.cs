using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUmbrella : MonoBehaviour
{
    public Transform test1;
    public Transform test2;
    public float clampedSpeed = 3f; // max speed when moving against umbrella
    public float clampingCurve = 250f; // max speed when moving against umbrella
    private Vector3 mousePos;
    private float mouseAngle;
    private bool facingRight = true;

    // Component References
    public Transform player;
    private Rigidbody2D playerRB;
    private Animator anim;

    void Start()
    {
        playerRB = player.gameObject.GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator> ();
    }

    void LateUpdate()
    {
        mousePos= Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10;
        mouseAngle = Vector3.Angle (Vector3.down, mousePos - transform.position);
        
        Debug.DrawLine(mousePos, transform.position);

        ClampVelocity();
        AnimateRotation();
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
        default: // idk
            anim.Play("down");
            break;
        }

        if ((mousePos - player.position).x > 0 && !facingRight)
            Flip ();
        else if ((mousePos - player.position).x < 0 && facingRight)
            Flip ();
    }

    void ClampVelocity(){
        float velocityAngle = Vector3.Angle(playerRB.velocity.normalized, Vector3.down);
        float clampingAngle = mouseAngle - velocityAngle - 90;

        float clampedVelocity = (clampingCurve / clampingAngle) - (clampingCurve / 90) + clampedSpeed; //smoothly transition between not clamping velocity and clamping it at clampedSpeed
/*
        Debug.Log(Vector3.Angle ((mousePos - transform.position).normalized, Vector3.down) - Vector3.Angle(playerRB.velocity.normalized, Vector3.down) - 90);
        Debug.DrawLine(playerRB.velocity.normalized, (mousePos - transform.position).normalized);
*/
        if(clampingAngle > 0)
            playerRB.velocity = Vector2.ClampMagnitude(playerRB.velocity, clampedVelocity);
    }

    public void Flip()
    {
        facingRight = !facingRight;
        player.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }
}
