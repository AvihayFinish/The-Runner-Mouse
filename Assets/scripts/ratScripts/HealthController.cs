using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    [Tooltip("Every object tagged with this tag will trigger the movement back to the starting point")]
    [SerializeField] string triggeringTag;
    [SerializeField] private Image healthBarFill;
    [SerializeField] string sceneName;
    [SerializeField] float damageAmount;
    [SerializeField] private Gradient colorGradient;
    private float MaxHealth = 3000;
    private float CurrentHelath;

    private void Awake(){
        CurrentHelath = MaxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.collider.tag == triggeringTag && enabled) {
        TakeDamage(damageAmount);
    }
}

    private void TakeDamage(float amount){
        CurrentHelath -= amount;
        CurrentHelath=Mathf.Clamp(CurrentHelath,0,MaxHealth);
        if(CurrentHelath ==0){ 
            if(ThisTheNewHighScore()){
                    PlayerPrefs.SetString("highscore", SceneManager.GetActiveScene().name);
                    SceneManager.LoadScene(sceneName);
                }
                else{ 
                    SceneManager.LoadScene(sceneName);
                }
        }
        UpdateHealthBar();
    }

    private void UpdateHealthBar(){
        healthBarFill.fillAmount = CurrentHelath / MaxHealth;
        healthBarFill.color = colorGradient.Evaluate(healthBarFill.fillAmount);
    }

    private bool ThisTheNewHighScore() {
    string highscore = PlayerPrefs.GetString("highscore");
    string currentSceneName = SceneManager.GetActiveScene().name;

    if (highscore.Contains("-") && currentSceneName.Contains("-")) {
        int highscoreDigit = int.Parse(highscore.Split('-')[1]);
        int currentSceneDigit = int.Parse(currentSceneName.Split('-')[1]);
        
        return currentSceneDigit > highscoreDigit;
    }

    return false;
}
}