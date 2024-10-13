using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Retalation : MonoBehaviour {
    public int score;
    public TunelSpawner tunelSpawner;
    public GameObject theRat;
    private float characterSpeed;
    private int index;
    private float baseSegment;
    private float startOfCurrentSegement;
    private float currentTarget;
    private float prevTarget;
    private float slope;
    private float n;
    public TextMeshProUGUI messageText;

    // Start is called before the first frame update
    void Start() {
        score = 0;
        messageText.text = "Score: " + score;
        index = 0;
        startOfCurrentSegement = -4;
        characterSpeed = PlayerPrefs.GetFloat("characterSpeed");
        baseSegment = characterSpeed * tunelSpawner.preporities[index].Time;
        currentTarget = tunelSpawner.preporities[index].Target / 100 * Camera.main.orthographicSize;
        prevTarget = currentTarget;
        slope = (currentTarget - prevTarget) / baseSegment;
        n = prevTarget - (slope * startOfCurrentSegement);
    }

    // Update is called once per frame
    void Update() {
        if (theRat.transform.position.x >= startOfCurrentSegement + baseSegment) {
            index += 1;
            startOfCurrentSegement += baseSegment;
            baseSegment = characterSpeed * tunelSpawner.preporities[index].Time;
            prevTarget = currentTarget;
            currentTarget = tunelSpawner.preporities[index].Target / 100 * Camera.main.orthographicSize;
            slope = (currentTarget - prevTarget) / baseSegment;
            n = prevTarget - (slope * startOfCurrentSegement);
            Debug.Log("index: " + index + " base: " + baseSegment + " target: " + currentTarget);
        }
        float centerY = slope * theRat.transform.position.x + n;
        float deltaY = theRat.transform.position.y - centerY;
        // Debug.Log("deltaY: " + deltaY);

        if (theRat.transform.position.y == centerY) {
            score += 10;
        }
        else if (MathF.Abs(deltaY) < 0.2f) {
            // Ensure deltaY is not too close to zero before taking Log10
            float safeDeltaY = MathF.Max(MathF.Abs(deltaY), 0.0001f); // Set a minimum threshold to avoid Log10(0) or near zero
            float logValue = MathF.Abs(MathF.Log10(safeDeltaY));
            score += (int)logValue;
        }
        messageText.text = "Score: " + score;
    }
}
