using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMover : MonoBehaviour
{
    [Tooltip("Speed of movement, in meters per second")]
    [SerializeField] float speed = 10f;

    // [SerializeField] InputAction moveHorizontal = new InputAction(type: InputActionType.Button);
    [SerializeField] InputAction moveVertical = new InputAction(type: InputActionType.Button);

    // private bool canMove = false;

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
        // moveHorizontal.Enable();
        moveVertical.Enable();
        // StartCoroutine(EnableMovementAfterDelay(7f));
    }

    void OnDisable()
    {
        // moveHorizontal.Disable();
        moveVertical.Disable();
    }

    void Update()
    {
        // if (canMove)
        // {
            // float horizontal = moveHorizontal.ReadValue<float>();
            float vertical = moveVertical.ReadValue<float>();
            Vector3 movementVector = new Vector3(0, vertical, 0) * speed * Time.deltaTime;
            transform.position += movementVector;
        // }
    }

    // IEnumerator EnableMovementAfterDelay(float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     canMove = true;
    // }
}