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
        string path = @"C:\Users\eladl\Downloads\finalProject\finalProject\Assets\tunelProperities.csv";
        preporities = ReadCsv(path);
        screenHeight = Camera.main.orthographicSize * 2f;
        GenerateTunnel(preporities);
    }

    public void GenerateTunnel(List<TunelPreporities> tunnelData) {
        float currentXPosition = -4;
        float previousTargetPosition = tunnelData[0].Target / 100.0f * screenHeight;

        for (int index = 0; index < tunnelData.Count; index++) {
            var entry = tunnelData[index];

            // Convert the precentege to decimal number between 0 to 1, and multiply it by the height of the screen to achive the precentege
            // from the screen that we want.
            float targetPosition = entry.Target / 100.0f * screenHeight;
            float gapSize = entry.Width / 100.0f * screenHeight;
            float halfGap = gapSize / 2.0f;
            float currentBottomY = previousTargetPosition - halfGap;
            float currentTopY = previousTargetPosition + halfGap;
            float nextBottomY = targetPosition - halfGap;
            float nextTopY = targetPosition + halfGap;
        
            // Calculate the base segment length based on time and character speed
            float baseSegmentLength = characterSpeed * entry.Time;

            // Create the slanted walls with the correct diagonal length
            CreateSlantedWall(bottomWallPrefab, currentXPosition, baseSegmentLength, currentBottomY, nextBottomY);
            CreateSlantedWall(topWallPrefab, currentXPosition, baseSegmentLength, currentTopY, nextTopY);

            // Handle the transition between intervals
            if (index < tunnelData.Count - 1) {
                float nextGapSize = tunnelData[index + 1].Width / 100.0f * screenHeight;
                float gapDifAbs = Mathf.Abs(gapSize - nextGapSize);
                float gapDif = gapSize - nextGapSize;

                // Only if there's a difference in the width
                if (gapDifAbs > 0.01f) {
                    // if the next width bigger than the current
                    if (gapDif < 0) {
                        CreateVerticalLine(new Vector3(currentXPosition + baseSegmentLength, nextBottomY, 0), 
                                        new Vector3(currentXPosition + baseSegmentLength, nextBottomY - (gapDifAbs / 2), 0), gapDif, true);
                        CreateVerticalLine(new Vector3(currentXPosition + baseSegmentLength, nextTopY, 0), 
                                        new Vector3(currentXPosition + baseSegmentLength, nextTopY + (gapDifAbs / 2), 0), gapDif, false);
                    } else {
                        CreateVerticalLine(new Vector3(currentXPosition + baseSegmentLength, nextBottomY, 0), 
                                        new Vector3(currentXPosition + baseSegmentLength, nextBottomY + (gapDifAbs / 2), 0), gapDif, true);

                        CreateVerticalLine(new Vector3(currentXPosition + baseSegmentLength, nextTopY, 0), 
                                        new Vector3(currentXPosition + baseSegmentLength, nextTopY - (gapDifAbs / 2), 0), gapDif, false);
                    }
                }
            }

            currentXPosition += baseSegmentLength;
            previousTargetPosition = targetPosition;
        }
    }

    private void CreateSlantedWall(GameObject wallPrefab, float startXPosition, float segmentLength, float startYPosition, float endYPosition) {
        // Calculate the diagonal distance (true length of the slanted wall)
        Vector3 startPoint = new(startXPosition, startYPosition, 0);
        Vector3 endPoint = new(startXPosition + segmentLength, endYPosition, 0);
        float diagonalDistance = Vector3.Distance(startPoint, endPoint);

        // Create a wall at the midpoint between the start and end points
        Vector3 middlePoint = (startPoint + endPoint) / 2;

        // Calculate the rotation based on the slope angle (atan2 of the yDifference and segmentLength)
        float actualSlopeAngle = Mathf.Atan2(endYPosition - startYPosition, segmentLength) * Mathf.Rad2Deg;

        // Create the wall with the proper rotation and position
        GameObject wall = Instantiate(wallPrefab, middlePoint, Quaternion.Euler(0, 0, actualSlopeAngle));

        // Adjust the scale to match the diagonal length (slanted length)
        wall.transform.localScale = new Vector3(diagonalDistance, 0.1f, 1);  // Set length as the diagonal distance
    }

    // Helper function to create vertical lines
    private void CreateVerticalLine(Vector3 start, Vector3 end, float gapDif, bool bottomOrTop) {
        float lineLength = Vector3.Distance(start, end);
        // Each side take half from the length of the vertical line. There is two options,  from current y go up or go down.
        Vector3 whereToPlacementPos = new(start.x, start.y + (lineLength / 2), start.z);
        Vector3 whereToPlacementNeg = new(start.x, start.y - (lineLength / 2), start.z);
        // If the current width bigger than the next, so we need from the bottom to go up and from the top to go down.
        if (gapDif > 0) {
            if (bottomOrTop) {
                GameObject verticalLine = Instantiate(bottomWallPrefab, whereToPlacementPos, Quaternion.identity);
                verticalLine.transform.localScale = new Vector3(0.1f, lineLength, 1);  // Adjust thickness and height.
            } else {
                GameObject verticalLine = Instantiate(topWallPrefab, whereToPlacementNeg, Quaternion.identity);
                verticalLine.transform.localScale = new Vector3(0.1f, lineLength, 1);
            }
        // If the current width smaller than the next, so we need from the bottom to go down and from the top to go up. 
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
        List<TunelPreporities> list = new();
        // Create easy first segment to simplify the start of the game for three seconds.
        TunelPreporities firstRecord = new()
        {
            Time = 3,
            Target = 0,
            Width = 40
        };

        list.Add(firstRecord);
        using var reader = new StreamReader(filePath);
        using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
        var records = csv.GetRecords<TunelPreporities>();
        list.AddRange(records);
        return list;
    }
}

public class TunelPreporities {
    public int Time { get; set; }
    public int Target { get; set; }
    public int Width { get; set; }
}