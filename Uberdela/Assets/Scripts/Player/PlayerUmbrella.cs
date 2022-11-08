using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUmbrella : MonoBehaviour
{
    public float clampedSpeed = 3f; // max speed when moving against umbrella
    public float clampingCurve = 250f; // max speed when moving against umbrella
    [HideInInspector]
    public Vector3 mousePos;
    private float mouseAngle;
    private bool facingRight = true;
    [HideInInspector]
    public bool active = true;

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

        if(active)
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
        float clampingAngle = Vector2.Angle(playerRB.velocity, mousePos - transform.position) - 90;
        float clampedVelocity = (clampingCurve / clampingAngle) - (clampingCurve / 90) + clampedSpeed; //smoothly transition between not clamping velocity and clamping it at clampedSpeed

        Debug.Log(Vector2.Angle(playerRB.velocity, mousePos - transform.position));
        Debug.DrawLine(transform.position, mousePos);
        Debug.DrawLine(transform.position, playerRB.velocity + (Vector2)transform.position);

        if(clampingAngle > 0){
            playerRB.velocity = Vector2.ClampMagnitude(playerRB.velocity, clampedVelocity);
        }
    }

    public void Flip()
    {
        facingRight = !facingRight;
        player.rotation = Quaternion.Euler(0, facingRight ? 0 : 180, 0);
    }
}
