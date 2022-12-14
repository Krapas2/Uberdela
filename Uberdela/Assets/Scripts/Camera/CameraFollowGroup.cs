using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollowGroup : MonoBehaviour
{
    public float minSize;
    public float maxSize;
    public Transform[] seguir;

    void Start()
    {
        transform.position = MediaPosicao(seguir) - Vector3.forward * 10;
    }

    void Update()
    {
        Vector3 variancia = VarianciaPosicao(seguir);
        float cameraTamanho = ((variancia.x > variancia.y) ? variancia.x : variancia.y)*1.5f;
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, Mathf.Clamp(cameraTamanho, minSize, maxSize), .9f * Time.deltaTime);

        Vector3 desiredPos = MediaPosicao(seguir);
        transform.position = Vector3.Lerp(transform.position, desiredPos, .9f * Time.deltaTime);
        transform.position = desiredPos - Vector3.forward * 10;
    }

    Vector3 MediaPosicao(Transform[] posicoes)
    {
        Vector3 r = Vector3.zero;
        foreach(Transform obj in posicoes){
            r += obj.position;
        }
        return r / posicoes.Length;
    }

    Vector3 VarianciaPosicao(Transform[] posicoes)
    {
        Vector3 media = MediaPosicao(posicoes);
        Vector3 r = Vector3.zero;
        foreach(Transform obj in posicoes){
            Vector3 j = obj.position - media;
            j = PotenciacaoVector3(j,2);
            r += j;
        }
        return PotenciacaoVector3(r / posicoes.Length, .5f);
    }
	
	Vector3 PotenciacaoVector3(Vector3 v, float exponente){
		return new Vector3(Mathf.Pow(v.x, exponente), Mathf.Pow(v.y, exponente), Mathf.Pow(v.z, exponente));
	}

    public void changeView(Transform[] newPoints){ 
        Camera cam = Camera.current.GetComponent<Camera>();
        CameraFollowGroup follow = cam.GetComponent<CameraFollowGroup>(); //we have to find the active camera for this to be usable with events
        if(follow){
            follow.seguir = newPoints;
        }else{
            Debug.Log("Cannot reference missing CameraFollowGroup");
        }
    }

    public void changeMinSize(CameraFollowGroup follow, float minSize){
        follow.minSize = minSize;
    }
    
    public void changeMaxSize(CameraFollowGroup follow, float maxSize){
        follow.maxSize = maxSize;
    }

    public void changeCurrentCameraMinSize(float minSize){
        Camera cam = Camera.current.GetComponent<Camera>();
        CameraFollowGroup follow = cam.GetComponent<CameraFollowGroup>(); //we have to find the active camera for this to be usable with events
        if(follow){
            changeMinSize(follow, minSize);
        }else{
            Debug.Log("Cannot reference missing CameraFollowGroup");
        }
    }
    public void changeCurrentCameraMaxSize(float maxSize){
        Camera cam = Camera.current.GetComponent<Camera>();
        CameraFollowGroup follow = cam.GetComponent<CameraFollowGroup>(); //we have to find the active camera for this to be usable with events
        if(follow){
            changeMaxSize(follow, maxSize);
        }else{
            Debug.Log("Cannot reference missing CameraFollowGroup");
        }
    }
    public void changeThisCameraMinSize(float minSize){
        changeMinSize(this, minSize);
    }
    public void changeThisCameraMaxSize(float maxSize){
        changeMaxSize(this, maxSize);
    }
}
