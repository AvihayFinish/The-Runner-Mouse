using System.Collections;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    // ----- Movement Settings -----
    [Header("Movement Settings")]
    [Tooltip("Speed of movement, in meters per second")]
    [SerializeField]public float speed = 10f;

    private Rigidbody2D  rb;  // Reference to the Rigidbody component

    [SerializeField] InputAction moveVertical = new InputAction(type: InputActionType.Button);

    // ----- Amadeo Device Connection -----
    [Header("Amadeo Device Connection")]
    public bool notGetForcesFromAmadeo = true;  // Flag to check if Amadeo device is connected or using keyboard
    
    void Start()
    {

        rb = GetComponent<Rigidbody2D >();
        // Make sure to set the Rigidbody's collision detection mode to Continuous for accurate collision handling
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;        
    }

    void Update()
    {
        if (notGetForcesFromAmadeo)
        {
            float vertical = moveVertical.ReadValue<float>();
            Vector3 movementVector = new Vector3(0, vertical, 0) * speed * Time.deltaTime;
            transform.position += movementVector;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public float GetSpeed()
    {
        return speed;
    }

    void OnEnable()
    {
        if(notGetForcesFromAmadeo)
            moveVertical.Enable();
    }

    void OnDisable()
    {
        moveVertical.Disable();
    }
}