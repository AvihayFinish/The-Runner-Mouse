using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retalation : MonoBehaviour {
    public float score;
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

    // Start is called before the first frame update
    void Start() {
        score = 0;
        index = 0;
        startOfCurrentSegement = -4;
        characterSpeed = PlayerPrefs.GetFloat("characterSpeed");
        baseSegment = characterSpeed * tunelSpawner.preporities[index].Time;
        currentTarget = tunelSpawner.preporities[index].Target;
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
            currentTarget = tunelSpawner.preporities[index].Target;
            slope = (currentTarget - prevTarget) / baseSegment;
            n = prevTarget - (slope * startOfCurrentSegement);
        }
        float centerY = slope * theRat.transform.position.x + n;
        float deltaY = theRat.transform.position.y - centerY;

        if (theRat.transform.position.y == centerY) {
            // Debug.Log("update score: " + 10);
            score += 10;
        }
        else if (MathF.Abs(deltaY) < 0.2f) {
            // Ensure deltaY is not too close to zero before taking Log10
            float safeDeltaY = MathF.Max(MathF.Abs(deltaY), 0.0001f); // Set a minimum threshold to avoid Log10(0) or near zero
            float logValue = MathF.Abs(MathF.Log10(safeDeltaY));
            // Debug.Log("update score: " + logValue);
            score += logValue;
        }
    }
}
