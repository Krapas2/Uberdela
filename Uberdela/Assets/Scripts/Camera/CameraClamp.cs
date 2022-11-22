using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClamp : MonoBehaviour
{
    public Vector2 minPos;
    public Vector2 maxPos;

    void LateUpdate()
    {
        float clampedX = Mathf.Clamp(transform.position.x, minPos.x, maxPos.x);
        float clampedY = Mathf.Clamp(transform.position.y, minPos.y, maxPos.y);
        transform.position = new Vector3(clampedX, clampedY, -10);
    }
}
