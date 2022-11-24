using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFunctions : MonoBehaviour
{
    public void Quit(){
        Application.Quit();
    }
    public void SetFlagTrue(string playerPref){
        PlayerPrefs.SetInt(playerPref, 1);
    }
    public void SetFlagFalse(string playerPref){
        PlayerPrefs.SetInt(playerPref, 0);
    }
}
