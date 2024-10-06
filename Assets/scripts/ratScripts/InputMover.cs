using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMover : MonoBehaviour {
    [Tooltip("Speed of movement, in meters per second")]
    [SerializeField] float speed;
    [SerializeField] InputAction moveVertical = new InputAction(type: InputActionType.Button);


    public void SetSpeed(float newSpeed) {
        speed = PlayerPrefs.GetFloat("characterSpeed");
    }

    public float GetSpeed() {
        return speed;
    }

    void OnEnable() {
        moveVertical.Enable();
    }

    void OnDisable() {
        moveVertical.Disable();
    }

    void Update() {
            float vertical = moveVertical.ReadValue<float>();
            Vector3 movementVector = new Vector3(0, vertical, 0) * speed * Time.deltaTime;
            transform.position += movementVector;
    }
}