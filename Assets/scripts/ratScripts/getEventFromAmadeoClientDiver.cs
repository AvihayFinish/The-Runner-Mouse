using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class getEventFromAmadeoClientDiver : MonoBehaviour
{
    // === Amadeo Client & Movement Parameters ===
    [Header("Amadeo Client")]
    [SerializeField] float factor_forces = 10f;  // Multiplier for forces received from the Amadeo device
    [SerializeField] public float idleUpwardSpeed;  // Speed for upward movement when no input is detected


    // === Internal State ===
    private InputMover im;  // Reference to the PlayerMovement script
    private int indexForce;  // Index of the selected finger (force to be used)
    private Rigidbody2D rb;  // Rigidbody component for physics-based movement


    [Header("Movement Settings")]
    [SerializeField] float verticalTolerance = 0.1f;  // Tolerance for vertical movement to avoid unnecessary small adjustments

    void Start()
    {
        im = GetComponent<InputMover>();  // Get the PlayerMovement script component
        rb = GetComponent<Rigidbody2D>();
    }

    //Subscribe to the OnForcesUpdated event when the object is enabled
    private void OnEnable()
    {   
        if(AmadeoClient.Instance != null)
            AmadeoClient.Instance.OnForcesUpdated += HandleForcesUpdated;  
    }

    // Unsubscribe from the OnForcesUpdated event when the object is disabled
    private void OnDisable()
    { 
        if(AmadeoClient.Instance != null)
            AmadeoClient.Instance.OnForcesUpdated -= HandleForcesUpdated;
    }


    //Handles the forces received from the Amadeo device
    public void HandleForcesUpdated(float[] forces)
    {   
        int indexForce = ParseStringToInt(PlayerPrefs.GetString("whichFinger"));
        // Ensure valid forces are received
        if (forces != null && forces.Length > 0)
        {
            im.notGetForcesFromAmadeo = false;  // Enable force reception from Amadeo

            // Calculate the new vertical position based on finger force
            float newVerticalPosition = forces[indexForce] * factor_forces;
            float currentVerticalPosition = transform.position.y;
            float verticalMovementSpeed;

            // Adjust vertical speed based on the difference between current and target vertical positions
            if (Mathf.Abs(newVerticalPosition - currentVerticalPosition) < verticalTolerance)
            {
                verticalMovementSpeed = idleUpwardSpeed;  // Apply idle upward speed if within tolerance
            }
            else
            {
                verticalMovementSpeed = Mathf.Sign(newVerticalPosition - currentVerticalPosition) * im.speed; // Move up or down
            }

            // Create a velocity vector using the vertical speed
            Vector2 targetVelocity = new Vector2(0, verticalMovementSpeed);

            // Apply the calculated velocity to the 2D Rigidbody
            rb.velocity = targetVelocity;
            im.notGetForcesFromAmadeo = true;  // Disable force reception after applying movement
        }
    }  

    private static int ParseStringToInt(string fingerString)
    {
        switch (fingerString)
        {
            case "thumb_right":
                return 0;
            case "indexFinger_right":
                return 1;
            case "middleFinger_right":
                return 2;
            case "ringFinger_right":
                return 3;
            case "littleFinger_right":
                return 4;
            case "littleFinger_left":
                return 4;
            case "ringFinger_left":
                return 3;
            case "middleFinger_left":
                return 2;
            case "indexFinger_left":
                return 1;
            case "thumb_left":
                return 0;
            default:
                throw new ArgumentException($"Invalid finger string: {fingerString}");
        }
    }
}
