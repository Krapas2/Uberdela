using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdArms : MonoBehaviour
{
    public Animator anim;

    [System.Serializable]
    public struct Atomo
    {
        public IdArms atomo;
        public float desiredDistance;
        public float k;

        public Atomo(IdArms atomo, float desiredDistance, float k, float b) {
            this.atomo = atomo;
            this. desiredDistance = desiredDistance;
            this.k = k;
        }
    }

    public Atomo[] associados = new Atomo[1];

    public float mass;

    public Vector3 accelerationVector;
    public Vector3 velocityVector;

    public Vector3 Fr;
    
    void Update()
    {
        UpdateVelocity();
        UpdateAcceleration();
        UpdateForce();

        transform.position += velocityVector * Time.deltaTime + (accelerationVector * (Time.deltaTime * Time.deltaTime))/2;

        if(velocityVector.x > 7.5){
            anim.Play("ArmOpen");
        } else{
            anim.Play("ArmClosed");
        }
    }
    void UpdateVelocity(){
        velocityVector += accelerationVector * Time.deltaTime;
    }

    void UpdateAcceleration(){
        accelerationVector = Fr/mass;
    }

    void UpdateForce()
    {
        Fr = Vector3.zero;
        
        foreach (Atomo atomo in associados)
        {
            Debug.DrawLine(transform.position, atomo.atomo.transform.position, Color.black);
            Vector3 curDistance = (atomo.atomo.transform.position - transform.position);
            Vector3 x = (curDistance.normalized * atomo.desiredDistance) - curDistance;
            Fr += -atomo.k*x;
        }
    }
}
