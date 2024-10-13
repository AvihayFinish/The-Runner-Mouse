using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class mainmenu : MonoBehaviour
{
    public bool amadeo = true;  // Flag to check if Amadeo device is connected or using keyboard

    public void PlayGame(){
        SceneManager.LoadSceneAsync(1);
        if(amadeo)
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