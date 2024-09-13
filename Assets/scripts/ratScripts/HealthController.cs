using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthController : MonoBehaviour
{
    [Tooltip("Every object tagged with this tag will trigger the movement back to the center point")]
    [SerializeField] string triggeringTag;
    [SerializeField] private Image healthBarFill;
    [SerializeField] string sceneName;
    [SerializeField] float damageAmount;
    [SerializeField] private Gradient colorGradient;
    private float MaxHealth = 3000;
    private float CurrentHelath;
        // Define a layer mask for the walls if needed
    [SerializeField] LayerMask wallLayer;

    private void Awake(){
        CurrentHelath = MaxHealth;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
    if (collision.collider.tag == triggeringTag && enabled) {
        TakeDamage(damageAmount);
        ResetPositionToCenter();
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

    // Function to reset the rat's position to the center dynamically using raycasting
    private void ResetPositionToCenter() {
        float maxDistance = 100f; // Maximum distance for raycasting
        Vector3 upDirection = Vector2.up;
        Vector3 downDirection = Vector2.down;

        // Raycast upward to find the closest object above
        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, upDirection, maxDistance, wallLayer);
        Debug.DrawRay(transform.position, upDirection * maxDistance, Color.red); // Draw the upward ray for debugging
        if (hitUp.collider != null) {
            Debug.Log($"Hit object above: {hitUp.collider.name} at point {hitUp.point.y}");
        } else {
            Debug.LogWarning("No object found above the player.");
        }

        // Raycast downward to find the closest object below
        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, downDirection, maxDistance, wallLayer);
        Debug.DrawRay(transform.position, downDirection * maxDistance, Color.green); // Draw the downward ray for debugging
        if (hitDown.collider != null) {
            Debug.Log($"Hit object below: {hitDown.collider.name} at point {hitDown.point.y}");
        } else {
            Debug.LogWarning("No object found below the player.");
        }
        if (hitUp.collider != null && hitDown.collider != null) {
            // Calculate the midpoint between the upper and lower objects
            float centerY = (hitUp.point.y + hitDown.point.y) / 2;
            
            // Reset the player's position to the calculated center (same x and z, center y)
            transform.position = new Vector3(transform.position.x, centerY, transform.position.z);
            Debug.Log($"Upper hit point: {hitUp.point.y}, Lower hit point: {hitDown.point.y}, Calculated center: {centerY}");

        } else if (hitUp.collider != null) {
            // If only an object above is detected, set to a little below it
            float offset = 0.5f; // Adjust offset to place the player slightly below the upper object
            transform.position = new Vector3(transform.position.x, hitUp.point.y - offset, transform.position.z);
        } else if (hitDown.collider != null) {
            // If only an object below is detected, set to a little above it
            float offset = 0.5f; // Adjust offset to place the player slightly above the lower object
            transform.position = new Vector3(transform.position.x, hitDown.point.y + offset, transform.position.z);
        } else {
            Debug.LogWarning("No objects found above or below the player.");
        }
    }

}