using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GoToTheEnd : MonoBehaviour {
    [SerializeField] string sceneName;
    // [SerializeField] Animator transitionanimator;
    public AudioSource audiosource;
    private InputMover otherinputMover;
    public Retalation forScore;
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Rat")) {
            Debug.Log("Current Score: " + forScore.score);
            PlayerPrefs.SetFloat("theScore", forScore.score);
            PlayerPrefs.Save();
            StartCoroutine(LoadLevel(other.GetComponent<InputMover>()));
        }
    }

    IEnumerator LoadLevel(InputMover inputmover){
        inputmover.enabled = false;
        // transitionanimator.SetTrigger("End");
        audiosource.Play();
        yield return new WaitForSeconds(0);
        UnLockedNewLevel();
        SceneManager.LoadScene(sceneName);  
        // transitionanimator.SetTrigger("Start");
    }

    void UnLockedNewLevel(){
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex")){
            PlayerPrefs.SetInt("ReachedIndex",SceneManager.GetActiveScene().buildIndex+1);
            PlayerPrefs.SetInt("UnlockedLevel",PlayerPrefs.GetInt("UnlockedLevel",1)+1);
            PlayerPrefs.Save();
        }
    }
}