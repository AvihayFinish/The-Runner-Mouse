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
    public AudioSource audiosource;
    private float MaxHealth = 3000;
    private float CurrentHelath;
    [SerializeField] LayerMask wallLayer;
    [SerializeField] private float invulnerabilityDuration = 3f;
    [SerializeField] private float blinkInterval = 0.2f; // Time between blinks

    private bool isInvulnerable = false; // Tracks if the rat is invulnerable
    private SpriteRenderer spriteRenderer;

    private void Awake(){
        CurrentHelath = MaxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer for blinking
    }


       private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(triggeringTag))
            {
                if (!isInvulnerable)
                {
                    TakeDamage(damageAmount);
                    audiosource.Play();
                    ResetPositionToCenter();
                    StartCoroutine(EnterInvulnerabilityState(invulnerabilityDuration)); // Pass duration here
                }
                else
                {
                    ResetPositionToCenter();
                }
            }
        }


        private void TakeDamage(float amount){
        if (!isInvulnerable) {
            CurrentHelath -= amount;
            CurrentHelath = Mathf.Clamp(CurrentHelath, 0, MaxHealth);
            if (CurrentHelath == 0) { 
                if (ThisTheNewHighScore()) {
                    PlayerPrefs.SetString("highscore", SceneManager.GetActiveScene().name);
                }
                SceneManager.LoadScene(sceneName);
            }
            UpdateHealthBar();
        }
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

    public void AddHealth(float amount)
{
    CurrentHelath += amount;
    CurrentHelath = Mathf.Clamp(CurrentHelath, 0, MaxHealth); // Ensure health doesn't exceed MaxHealth
    UpdateHealthBar(); // Update the health bar UI
}


    // Function to reset the rat's position to the center dynamically using raycasting
    private void ResetPositionToCenter() {
        float maxDistance = 100f; // Maximum distance for raycasting
        Vector3 upDirection = Vector2.up;
        Vector3 downDirection = Vector2.down;

        RaycastHit2D hitUp = Physics2D.Raycast(transform.position, upDirection, maxDistance, wallLayer);
        Debug.DrawRay(transform.position, upDirection * maxDistance, Color.red); // Draw the upward ray for debugging
        if (hitUp.collider != null) {
            Debug.Log($"Hit object above: {hitUp.collider.name} at point {hitUp.point.y}");
        } else {
            Debug.LogWarning("No object found above the player.");
        }

        RaycastHit2D hitDown = Physics2D.Raycast(transform.position, downDirection, maxDistance, wallLayer);
        Debug.DrawRay(transform.position, downDirection * maxDistance, Color.green); // Draw the downward ray for debugging
        if (hitDown.collider != null) {
            Debug.Log($"Hit object below: {hitDown.collider.name} at point {hitDown.point.y}");
        } else {
            Debug.LogWarning("No object found below the player.");
        }
        
        if (hitUp.collider != null && hitDown.collider != null) {
            float centerY = (hitUp.point.y + hitDown.point.y) / 2;
            transform.position = new Vector3(transform.position.x, centerY, transform.position.z);
            Debug.Log($"Upper hit point: {hitUp.point.y}, Lower hit point: {hitDown.point.y}, Calculated center: {centerY}");
        } else if (hitUp.collider != null) {
            float offset = 0.5f;
            transform.position = new Vector3(transform.position.x, hitUp.point.y - offset, transform.position.z);
        } else if (hitDown.collider != null) {
            float offset = 0.5f;
            transform.position = new Vector3(transform.position.x, hitDown.point.y + offset, transform.position.z);
        } else {
            Debug.LogWarning("No objects found above or below the player.");
        }
    }

    // Coroutine to enter invulnerability state and handle blinking
    public IEnumerator EnterInvulnerabilityState(float duration)
{
    isInvulnerable = true; // מסמן שהעכבר בלתי פגיע
    float timer = 0f;
    while (timer < duration)
    {
        // מחליף את הנראות של הספראיט כדי ליצור אפקט מהבהב
        spriteRenderer.enabled = !spriteRenderer.enabled;
        yield return new WaitForSeconds(blinkInterval);
        timer += blinkInterval; // מעדכן את הספירה
    }
    // מבטיח שהספראיט יהיה נראה בסוף
    spriteRenderer.enabled = true;
    isInvulnerable = false; // מסמן שהעכבר חוזר להיות פגיע
}

}