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
    [SerializeField] private AmadeoClient amadeoClient;  // Reference to the AmadeoClient script
    [SerializeField] float factor_forces = 1f;  // Multiplier for forces received from the Amadeo device

    // === Internal State ===
    private Rigidbody2D rb;  // Rigidbody component for physics-based movement
    private PlayerMovement pm;  // Reference to the PlayerMovement script
    private int indexForce = 3;  // Index of the selected finger (force to be used) *****from the settings file*****

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();  // Get the Rigidbody component attached to the GameObject
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;  // Set collision detection mode to Continuous for better accuracy
        pm = GetComponent<PlayerMovement>();  // Get the PlayerMovement script component
    }

    //Subscribe to the OnForcesUpdated event when the object is enabled
    private void OnEnable()
    {
        if (amadeoClient != null)
        {
            amadeoClient.OnForcesUpdated += HandleForcesUpdated;
        }
    }

    // Unsubscribe from the OnForcesUpdated event when the object is disabled
    private void OnDisable()
    {
        if (amadeoClient != null)
        {
            amadeoClient.OnForcesUpdated -= HandleForcesUpdated;
        }
    }


    //Handles the forces received from the Amadeo device
    private void HandleForcesUpdated(float[] forces)
    {
        Debug.Log("indexForce" + indexForce);
        Debug.Log("forces" + forces);

        // Ensure valid forces are received
        if (forces != null && forces.Length > 0)
        {
            pm.notGetForcesFromAmadeo = false;  // Enable force reception from Amadeo

            float vertical = forces[indexForce];
            Vector3 movementVector = new Vector3(0, vertical, 0) * pm.speed * Time.deltaTime;
            transform.position += movementVector;

            pm.notGetForcesFromAmadeo = true;  // Disable force reception after applying movement
        }
    }    
}
