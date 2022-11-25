using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEvent : MonoBehaviour
{
    public LayerMask player;
    public UnityEvent _event;

    void Start()
    {
        if (_event == null)
            _event = new UnityEvent();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if ((player & (1 << col.gameObject.layer)) != 0){
            _event.Invoke();
        }
    }
}
