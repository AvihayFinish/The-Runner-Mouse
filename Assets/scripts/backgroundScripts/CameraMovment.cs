using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovment : MonoBehaviour
{
    [SerializeField] public float CameraSpeed;

    void Start() {
        CameraSpeed = PlayerPrefs.GetFloat("characterSpeed");
    }

    void Update() {
        transform.position += new Vector3(CameraSpeed*Time.deltaTime,0,0);
    }
}