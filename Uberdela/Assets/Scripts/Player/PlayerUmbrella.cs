using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUmbrella : MonoBehaviour
{

    Vector3 mousePos;
    private float mouseAngle;

    // Component References
    public PlayerMov mov;
    private Animator anim;

    void Start()
    {
		anim = GetComponent<Animator> ();
    }

    void Update()
    {
        mousePos= Camera.main.ScreenToWorldPoint (Input.mousePosition) + Vector3.forward * 10;
        mouseAngle = Vector3.Angle (Vector3.down, mousePos - transform.position);
        
        
        Debug.DrawLine(mousePos,transform.position);

        AnimateRotation();
    }

    void AnimateRotation(){

        int treatedAngle = (int)mouseAngle / 36;
        Debug.Log(mouseAngle + " " + treatedAngle);
        
        switch(treatedAngle){
        case 0:
            anim.Play("down");
            break;
        case 1:
            anim.Play("diagonalDown");
            break;
        case 2:
            anim.Play("straight");
            break;
        case 3:
            anim.Play("diagonalUp");
            break;
        case 4:
            anim.Play("up");
            break;
        default:
            anim.Play("down");
            break;
        }
        if ((mousePos - transform.position).x > 0 && !mov.facingRight)
            mov.Flip ();
        else if ((mousePos - transform.position).x < 0 && mov.facingRight)
            mov.Flip ();
    }
}
