using System.Collections; // Ensure this is included for IEnumerator
using UnityEngine;

public class GetHighSpeed : MonoBehaviour
{
    public AudioSource audiosource;
    public float speedMultiplier = 2f;
    public float effectDuration = 5f;

    private Scrolling backgroundScrolling;
    private CameraMovment cameraMovement;
    private SpriteRenderer spriteRenderer; // Reference to the sprite

    private void Start()
    {
        // Get references to the scripts controlling background, camera, and the sprite
        backgroundScrolling = FindObjectOfType<Scrolling>();
        cameraMovement = FindObjectOfType<CameraMovment>();
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the sprite renderer attached to the object
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Rat"))
        {
            audiosource.Play();

            // Hide the icon (sprite) immediately
            if (spriteRenderer != null)
                spriteRenderer.enabled = false;

            StartCoroutine(HighSpeedEffect());
        }
    }

    private IEnumerator HighSpeedEffect()
    {
        // Double the speed of the camera and background
        if (backgroundScrolling != null)
            backgroundScrolling.speed /= speedMultiplier;

        if (cameraMovement != null)
            cameraMovement.CameraSpeed /= speedMultiplier;

        // Wait for 5 seconds
        yield return new WaitForSeconds(effectDuration);

        // Reset the speeds back to their original values
        if (backgroundScrolling != null)
            backgroundScrolling.speed *= speedMultiplier;

        if (cameraMovement != null)
            cameraMovement.CameraSpeed *= speedMultiplier;

        // Finally, deactivate the object fully after the effect finishes
        gameObject.SetActive(false);
    }
}
