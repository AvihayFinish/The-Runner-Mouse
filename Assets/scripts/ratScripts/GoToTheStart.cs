// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GoToTheStart : MonoBehaviour {
//     [Tooltip("Every object tagged with this tag will trigger the movement back to the starting point")]
//     // [SerializeField] string triggeringTag;
//     public AudioSource audiosource;
//     private Vector3 startPosition;
//     private Vector3 startSize;
//     private bool isReturning = false; 
//     private Renderer otherRenderer; 
//     private InputMover inputMover;
//     private float initialSpeed;
//     private GameObject[] potions;
//     private GameObject[] tunnelTops;
//     private GameObject[] tunnelBottoms;

//     private void Start() {
//         startPosition = transform.position;
//         startSize = transform.localScale;
//         inputMover = GetComponent<InputMover>();
//         initialSpeed = inputMover.GetSpeed();
//         // potions = GameObject.FindGameObjectsWithTag("Potions");

//         // Get all tunnelTop and tunnelBottom objects in the scene
//         tunnelTops = GameObject.FindGameObjectsWithTag("TunnelTop");
//         tunnelBottoms = GameObject.FindGameObjectsWithTag("TunnelBottom");
//     }

//     private void OnTriggerEnter2D(Collider2D other) {
//         // Check if the tag is either "TunnelTop" or "TunnelBottom"
//         if ((other.CompareTag("TunnelTop") || other.CompareTag("TunnelBottom")) && enabled && !isReturning) {
//             StartCoroutine(PushToMiddle(other.GetComponent<Renderer>(), this.GetComponent<InputMover>()));
//             audiosource.Play();
//         }
//     }

//     private IEnumerator PushToMiddle(Renderer renderer, InputMover mover) {
//         isReturning = true;
//         renderer.enabled = true;
//         mover.enabled = false;

//         // Find the nearest walls above and below the mouse
//         float mouseY = transform.position.y;
//         GameObject nearestTop = FindNearestWallAbove(mouseY);
//         GameObject nearestBottom = FindNearestWallBelow(mouseY);

//         if (nearestTop != null && nearestBottom != null) {
//             // Calculate the middle position between the nearest top and bottom walls
//             float middleY = (nearestTop.transform.position.y + nearestBottom.transform.position.y) / 2;
//             Vector3 middlePosition = new Vector3(transform.position.x, middleY, transform.position.z);

//             // Move the mouse to the middle position
//             transform.position = middlePosition;
//         }

//         yield return new WaitForSeconds(0.5f);

//         mover.enabled = true;
//         renderer.enabled = false;
//         isReturning = false;
//     }

//     private GameObject FindNearestWallAbove(float mouseY) {
//         GameObject nearestTop = null;
//         float minDistance = Mathf.Infinity;

//         foreach (var top in tunnelTops) { // Corrected variable name
//             float distance = top.transform.position.y - mouseY;
//             if (distance > 0 && distance < minDistance) {
//                 minDistance = distance;
//                 nearestTop = top;
//             }
//         }

//         return nearestTop;
//     }

//     private GameObject FindNearestWallBelow(float mouseY) {
//         GameObject nearestBottom = null;
//         float minDistance = Mathf.Infinity;

//         foreach (var bottom in tunnelBottoms) { // Corrected variable name
//             float distance = mouseY - bottom.transform.position.y;
//             if (distance > 0 && distance < minDistance) {
//                 minDistance = distance;
//                 nearestBottom = bottom;
//             }
//         }

//         return nearestBottom;
//     }

//     private void ReturnToStart() {
//         transform.position = startPosition;
//         transform.localScale = startSize;
//         inputMover.SetSpeed(initialSpeed);
//         foreach (var potion in potions) {
//             if (!potion.activeSelf)
//                 potion.SetActive(true);
//         }
//     }

//     private void Update() {
//     }
// }