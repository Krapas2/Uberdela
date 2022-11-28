using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public void EnterGame(){
        if(PlayerPrefs.HasKey("playedIntro")){
            if(PlayerPrefs.GetInt("playedIntro") == 1){
                SceneManager.LoadScene(2);
            }else{
                SceneManager.LoadScene(1);
            }
        }else{
            SceneManager.LoadScene(1);
        }
    }
    public void LoadScene(string scene){
        SceneManager.LoadScene(scene);
    }
    public void LoadScene(int scene){
        SceneManager.LoadScene(scene);
    }
}
