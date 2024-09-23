using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class highscoreController : MonoBehaviour
{
    [SerializeField] TMP_Text HSText;
    void Start()
    {
        HSText.text = "High Score: " + PlayerPrefs.GetString("highscore"); 
           }

    void Update()
    {
        
    }
}
