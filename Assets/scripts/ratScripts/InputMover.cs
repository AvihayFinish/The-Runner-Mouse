using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMover : MonoBehaviour {
    [Tooltip("Speed of movement, in meters per second")]
    [SerializeField]public float speed;
    [SerializeField] InputAction moveVertical = new InputAction(type: InputActionType.Button);
     // ----- Amadeo Device Connection -----

    internal bool notGetForcesFromAmadeo = false;  // Flag to check if Amadeo device is connected or using keyboard




    public void SetSpeed(float newSpeed) {
        speed = PlayerPrefs.GetFloat("characterSpeed");
    }

    public float GetSpeed() {
        return speed;
    }

    void OnEnable() {
        if(notGetForcesFromAmadeo)
            moveVertical.Enable();
    }

    void OnDisable() {
        if(notGetForcesFromAmadeo)
            moveVertical.Disable();
    }

    void Update() {
        if(notGetForcesFromAmadeo)
        {
            float vertical = moveVertical.ReadValue<float>();
            Vector3 movementVector = new Vector3(0, vertical, 0) * speed * Time.deltaTime;
            transform.position += movementVector;
        }
    }
}