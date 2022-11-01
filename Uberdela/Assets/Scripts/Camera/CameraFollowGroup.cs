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
        
    }

    void LateUpdate()
    {
        Vector3 variancia = VarianciaPosicao(seguir);
        float cameraTamanho = ((variancia.x > variancia.y) ? variancia.x : variancia.y)*1.5f;
        Camera.main.orthographicSize = Mathf.Clamp(cameraTamanho, minSize, maxSize);

        transform.position = MediaPosicao(seguir);
        transform.position -= Vector3.forward * 10;
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
}
