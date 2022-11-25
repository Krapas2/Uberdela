using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLoadArea : MonoBehaviour
{

    //Gameobjects should contain area for loading and unloading
    public GameObject loadObject; 
    public GameObject unloadObject;
    public LayerMask triggers;

    public void Load(){
        loadObject.SetActive(true);
        unloadObject.SetActive(false);
    }
}
