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
    [SerializeField] float factor_forces = 100f;  // Multiplier for forces received from the Amadeo device

    // === Internal State ===
    private InputMover im;  // Reference to the PlayerMovement script
    private int indexForce;  // Index of the selected finger (force to be used)

    void Start()
    {
        int indexForce = ParseStringToInt(PlayerPrefs.GetString("whichFinger"));
        Debug.Log("whichFinger" + indexForce);
        im = GetComponent<InputMover>();  // Get the PlayerMovement script component
    }

    //Subscribe to the OnForcesUpdated event when the object is enabled
    private void OnEnable()
    {   
        AmadeoClient.Instance.OnForcesUpdated += HandleForcesUpdated;  
    }

    // Unsubscribe from the OnForcesUpdated event when the object is disabled
    private void OnDisable()
    { 
        AmadeoClient.Instance.OnForcesUpdated -= HandleForcesUpdated;
    }


    //Handles the forces received from the Amadeo device
    public void HandleForcesUpdated(float[] forces)
    {
        Debug.Log("indexForce111" + indexForce);
        Debug.Log("forces" + forces);

        // Ensure valid forces are received
        if (forces != null && forces.Length > 0)
        {
            im.notGetForcesFromAmadeo = false;  // Enable force reception from Amadeo

            float vertical = forces[1];
            Vector3 movementVector = new Vector3(0, vertical, 0) * im.speed * factor_forces * Time.deltaTime;
            transform.position += movementVector;

            im.notGetForcesFromAmadeo = true;  // Disable force reception after applying movement
        }
    }  

    public enum RightFingersEnum
    {
        thumb_right = 4,
        indexFinger_right = 3,
        middleFinger_right = 2,
        ringFinger_right = 1,
        littleFinger_right = 0
    }  

    public enum LeftFingersEnum
    {
        littleFinger_left = 0,
        ringFinger_left = 1,
        middleFinger_left = 2,
        indexFinger_left = 3,
        thumb_left = 4
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
                return 0;
            case "ringFinger_left":
                return 1;
            case "middleFinger_left":
                return 2;
            case "indexFinger_left":
                return 3;
            case "thumb_left":
                return 4;
            default:
                throw new ArgumentException($"Invalid finger string: {fingerString}");
        }
    }
}
