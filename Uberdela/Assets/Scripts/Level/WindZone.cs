using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindZone : MonoBehaviour
{
    public float maxSpeed; // max speed objects can reach
    public float acceleration; // acceleration per frame spent in zone
    public LayerMask affects; // objects affected by wind

    public int umbrellaAlignmentCurve = 1;

    private PlayerUmbrella umbrella;

    void Start()
    {
        umbrella = FindObjectOfType<PlayerUmbrella>();
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
        if ((affects & (1 << col.gameObject.layer)) != 0 && rb)
        {
            //rb.gravityScale = 0;
            rb.drag = .15f;
            if(umbrella){
                if(umbrella.transform.parent.gameObject == col.gameObject)
                    umbrella.active = false;
            }
        }
    }
    void OnTriggerStay2D(Collider2D col)
    {
        Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
        if ((affects & (1 << col.gameObject.layer)) != 0 && rb)
        {
            if(umbrella){
                if(umbrella.transform.parent.gameObject == col.gameObject){
                    if(Input.GetButton("Fire1")){
                        float clampingAngle = Vector2.Angle(-transform.up, umbrella.mousePos - transform.position);
                        float umbrellaAlignment = Mathf.Pow(clampingAngle, umbrellaAlignmentCurve)/Mathf.Pow(180, umbrellaAlignmentCurve);

                        Vector2 speedToAdd = Vector2.ClampMagnitude(transform.up * acceleration * Time.deltaTime * umbrellaAlignment, maxSpeed * umbrellaAlignment);

                        //rb.gravityScale = 1-umbrellaAlignment;
                        rb.AddForce(speedToAdd, ForceMode2D.Impulse); 
                    }
                } else{
                    rb.AddForce(transform.up * acceleration, ForceMode2D.Impulse); 
                    rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
                }
            }else{
                rb.AddForce(transform.up * acceleration, ForceMode2D.Impulse); //ponteiro com o rb não precisaria repetir
                rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        Rigidbody2D rb = col.gameObject.GetComponent<Rigidbody2D>();
        if ((affects & (1 << col.gameObject.layer)) != 0 && rb)
        {
            //rb.gravityScale = 1;
            rb.drag = 0f;
            if(umbrella){
                if(umbrella.transform.parent.gameObject == col.gameObject)
                    umbrella.active = true;
            }
        }
    }
}
