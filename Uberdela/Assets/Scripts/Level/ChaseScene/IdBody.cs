using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdBody : MonoBehaviour
{
    public Transform[] arms;
    private Vector3 offset;

    void Start()
    {
        offset = MediaPosicao(arms) - transform.position;
    }

    void Update()
    {
        transform.position = MediaPosicao(arms) - offset;
    }

    
    Vector3 MediaPosicao(Transform[] posicoes)
    {
        Vector3 r = Vector3.zero;
        foreach(Transform obj in posicoes){
            r += obj.position;
        }
        return r / posicoes.Length;
    }
}
