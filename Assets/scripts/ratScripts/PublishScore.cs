using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PublishScore : MonoBehaviour {
    public TextMeshProUGUI messageText;
    // Start is called before the first frame update
    void Start() {
        messageText.text += PlayerPrefs.GetFloat("theScore");
    }
}
