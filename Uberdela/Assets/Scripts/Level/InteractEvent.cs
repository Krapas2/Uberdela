using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractEvent : MonoBehaviour
{

    public float range;
    public LayerMask player;
    public UnityEvent _event;

    void Start()
    {
        if (_event == null)
            _event = new UnityEvent();
    }

    void Update()
    {
        if(Input.GetButtonDown("Interact")){
            if(Physics2D.OverlapCircle(transform.position, range, player)){
                _event.Invoke();
            }
        }
    }
}
