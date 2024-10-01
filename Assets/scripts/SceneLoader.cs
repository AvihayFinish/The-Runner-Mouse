using UnityEngine;
using UnityEngine.SceneManagement;  // Required for scene management

public class SceneLoader : MonoBehaviour
{
    // This method can be called when the Play button is clicked
    public void LoadNextScene()
    {
        // Replace "NextSceneName" with the actual name of your next scene
        SceneManager.LoadScene("Main Menu"); 
    }
}
