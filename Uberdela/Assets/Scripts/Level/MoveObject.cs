using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
    public void move(Transform moves){
        moves.transform.position = transform.position;
    }
}