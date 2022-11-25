using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraPoints : MonoBehaviour
{
    public void changeView(Transform[] newPoints){
        Camera cam = Camera.current.GetComponent<Camera>();
        CameraFollowGroup follow = cam.GetComponent<CameraFollowGroup>();
        if(follow){
            follow.seguir = newPoints;
        }else{
            Debug.Log("Cannot reference missing CameraFollowGroup");
        }
    }
}
