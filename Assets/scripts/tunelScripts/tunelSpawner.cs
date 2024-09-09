using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;

public class tunelSpawner : MonoBehaviour {
    public GameObject topWallPrefab;
    public GameObject bottomWallPrefab; 
    public float characterSpeed;  
    public float screenHeight;
    List<TunelPreporities> preporities;

    void Start() {
        string path = @"C:\unityProjects\theBird\finalProject\Assets\tunelProperities.csv";
        preporities = ReadCsv(path);
        screenHeight = Camera.main.orthographicSize * 2f;
        GenerateTunnel(preporities);
    }

    public void GenerateTunnel(List<TunelPreporities> tunnelData) {
        float currentXPosition = 0;   // Start the tunnel from the origin on the x-axis
        // Set the initial target position correctly
        float previousTargetPosition = tunnelData[0].Target / 100.0f * screenHeight;  // Ensure the tunnel starts at the first target position
        float previousGapSize = tunnelData[0].Width / 100.0f * screenHeight;  // Initial width (percentage of screen height)

        for (int index = 0; index < tunnelData.Count; index++) {
            var entry = tunnelData[index];
            float time = entry.Time;         // Time for the character to pass this segment
            float target = entry.Target;     // Target position (percentage of screen height)
            float width = entry.Width;       // Width of the tunnel gap (percentage of screen height)

            // Calculate the actual target position and gap size
            float targetPosition = target / 100.0f * screenHeight;
            float gapSize = width / 100.0f * screenHeight;
            float halfGap = gapSize / 2.0f;

            // Calculate the length of this tunnel segment
            float segmentLength = Mathf.Round(characterSpeed * time * 1000f) / 1000f;
            // Debug.Log($"Segment {index}: StartX: {currentXPosition}, Segment Length: {segmentLength}, EndX: {currentXPosition + segmentLength}");

            // Calculate the slope for the rotation (how much the walls tilt)
            float yDifference = targetPosition - previousTargetPosition;

            // Ensure no slope is applied if the target position is the same as the previous one
            float slopeAngle = Mathf.Approximately(yDifference, 0) ? 0 : Mathf.Atan2(yDifference, segmentLength) * Mathf.Rad2Deg;

            // Create the bottom and top walls with Z-axis rotation
            CreateSlantedWall(bottomWallPrefab, currentXPosition, segmentLength, previousTargetPosition - halfGap, targetPosition - halfGap, slopeAngle);
            CreateSlantedWall(topWallPrefab, currentXPosition, segmentLength, previousTargetPosition + halfGap, targetPosition + halfGap, slopeAngle);
        
            // Only add vertical lines if the width changes between consecutive intervals
            if (index < tunnelData.Count - 1) {
                float nextGapSize = tunnelData[index + 1].Width / 100.0f * screenHeight;

                float currentBottom = targetPosition - halfGap;
                float currentTop = targetPosition + halfGap;

                float gapDifAbs = Mathf.Abs(gapSize - nextGapSize);
                float gapDif = gapSize - nextGapSize;

                // Only if there's a difference in the width
                if (gapDifAbs > 0.01f) {
                    if (gapDif < 0) {
                        // Calculate where the next interval will start, and create vertical lines to connect
                        // bottom of current interval to bottom of next interval
                        CreateVerticalLine(new Vector3(currentXPosition + segmentLength, currentBottom, 0), 
                                        new Vector3(currentXPosition + segmentLength, currentBottom - (gapDifAbs / 2), 0), gapDif, true);
                        // top of current interval to top of next interval
                        CreateVerticalLine(new Vector3(currentXPosition + segmentLength, currentTop, 0), 
                                        new Vector3(currentXPosition + segmentLength, currentTop + (gapDifAbs / 2), 0), gapDif, false);
                    } else {
                        CreateVerticalLine(new Vector3(currentXPosition + segmentLength, currentBottom, 0), 
                                        new Vector3(currentXPosition + segmentLength, currentBottom + (gapDifAbs / 2), 0), gapDif, true);

                        CreateVerticalLine(new Vector3(currentXPosition + segmentLength, currentTop, 0), 
                                        new Vector3(currentXPosition + segmentLength, currentTop - (gapDifAbs / 2), 0), gapDif, false);
                    }
                }
            }

        // Move to the next segment's start position
        currentXPosition = Mathf.Round((currentXPosition + segmentLength) * 1000f) / 1000f;

        // Update the previous target position and gap size
        previousTargetPosition = targetPosition;
        previousGapSize = gapSize;
        }
    }

    private void CreateSlantedWall(GameObject wallPrefab, float startXPosition, float segmentLength, float startYPosition, float endYPosition,
                                    float slopeAngle) {
        Vector3 startPoint = new(startXPosition, startYPosition, 0);
        Vector3 endPoint = new(startXPosition + segmentLength, endYPosition, 0);
        Vector3 middlePoint = (startPoint + endPoint) / 2;
        // Debug.Log("the middle: " + middlePoint);

        // Calculate the rotation based on the slope angle
        Quaternion rotation = Quaternion.Euler(0, 0, slopeAngle);

        // Create the wall with rotation applied
        GameObject wall = Instantiate(wallPrefab, middlePoint, rotation);
        wall.transform.localScale = new Vector3(segmentLength, 0.1f, 1);  // Adjust thickness and length as necessary
    }

    // Helper function to create vertical lines
    private void CreateVerticalLine(Vector3 start, Vector3 end, float gapDif, bool bottomOrTop) {
        float lineLength = Vector3.Distance(start, end);
        Vector3 whereToPlacementPos = new(start.x, start.y + (lineLength / 2), start.z);
        Vector3 whereToPlacementNeg = new(start.x, start.y - (lineLength / 2), start.z);
        if (gapDif > 0) {
            if (bottomOrTop) {
                GameObject verticalLine = Instantiate(bottomWallPrefab, whereToPlacementPos, Quaternion.identity);
                verticalLine.transform.localScale = new Vector3(0.1f, lineLength, 1);  // Adjust thickness and height
            } else {
                GameObject verticalLine = Instantiate(topWallPrefab, whereToPlacementNeg, Quaternion.identity);
                verticalLine.transform.localScale = new Vector3(0.1f, lineLength, 1);
            }
        } else {
            if (bottomOrTop) {
                GameObject verticalLine = Instantiate(bottomWallPrefab, whereToPlacementNeg, Quaternion.identity);
                verticalLine.transform.localScale = new Vector3(0.1f, lineLength, 1);
            } else {
                GameObject verticalLine = Instantiate(topWallPrefab, whereToPlacementPos, Quaternion.identity);
                verticalLine.transform.localScale = new Vector3(0.1f, lineLength, 1);
            }
        }
    }

    public static List<TunelPreporities> ReadCsv(string filePath) {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture))) {
            var records = csv.GetRecords<TunelPreporities>();
            return new List<TunelPreporities>(records);
        }
    }
}

public class TunelPreporities {
    public int Time { get; set; }
    public int Target { get; set; }
    public int Width { get; set; }
}