using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class mainmenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadSceneAsync(1);
        if( AmadeoClient.Instance.isAmadeo)
            AmadeoClient.Instance.StartReceiveData();
    }

    public void QuitGame(){

        Application.Quit();
        
    }
    public void sceneRat()
    {
        SceneManager.LoadSceneAsync("Scene_Rat");

    }
   
}