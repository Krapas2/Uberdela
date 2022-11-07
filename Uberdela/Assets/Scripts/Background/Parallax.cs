using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float factor;

    private Transform cam;
    
    private Vector3 origPos;
    private Vector3 origCamPos;

    void Start()
    {
        cam = FindObjectOfType<Camera>().transform;

        origPos = transform.position;
        origCamPos = cam.position;
    }

    void Update()
    {
        if(factor < 1)      // se o objeto estiver longe ele se move junto da camera
            transform.position = origPos + Lerp(origCamPos, cam.position, factor);
        else                // se estiver perto ele se move contra a camera (arvores em https://youtu.be/LYS8Ef17E5g?t=10)
            transform.position = origPos - Lerp(origCamPos, cam.position, factor);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public static Vector3 Lerp(Vector3 a, Vector3 b, float t ){
        return t*b + (1-t)*a;
    }
}
