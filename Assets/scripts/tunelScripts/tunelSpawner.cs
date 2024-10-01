using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class tunelSpawner : MonoBehaviour {
    public GameObject topWallPrefab;
    public GameObject bottomWallPrefab; 
    public GameObject bonusOne;
    public GameObject bonusTwo;
    public GameObject bonusThree;
    public GameObject bonusFour;
    public GameObject endTunnel;
    public GameObject PauseObject;
    public TextMeshProUGUI messageText;
    public float characterSpeed;  
    public float screenHeight;
    List<TunelPreporities> preporities;

    void Start() {
        string path = @"C:\unityProjects\theBird\finalProject\Assets\tunelProperities.csv";
        PauseObject.SetActive(false);
        preporities = ReadCsv(path);
        screenHeight = Camera.main.orthographicSize * 2f;
        GenerateTunnel(preporities);
    }

    public void GenerateTunnel(List<TunelPreporities> tunnelData) {
        float currentXPosition = -4;
        float previousTargetPosition = tunnelData[0].Target / 100.0f * screenHeight;

        for (int index = 0; index < tunnelData.Count; index++) {
            var entry = tunnelData[index];

            // Get the orthographic size (which is half the screen height)
            float halfScreenHeight = Camera.main.orthographicSize;

            // Calculate the target position relative to the middle of the screen
            float targetPosition = entry.Target / 100f * halfScreenHeight;

            // Clamp the position to ensure it stays within the screen bounds
            targetPosition = Mathf.Clamp(targetPosition, -halfScreenHeight, halfScreenHeight);
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

            if (entry.Bonus) {
                float xPosition = Random.Range(currentXPosition, currentXPosition + baseSegmentLength); // Choose random x in the segment to put the bonus
                float slope = (targetPosition - previousTargetPosition) / baseSegmentLength; // Find the slope of the line
                float n = previousTargetPosition - (slope * currentXPosition); // Find n by put point in the equation
                float yPosition = (slope * xPosition) + n; // Find the corresponding y for the randon x in the segment
                Vector3 bonusPosition = new(xPosition, yPosition, 0);

                int[] bonusOptions = { 1, 2, 3, 4 };
                int randomIndex = Random.Range(0, bonusOptions.Length);
                int randomNumber = bonusOptions[randomIndex]; // Random choose between four bonuses
                switch (randomNumber) {
                    case 1:
                        Instantiate(bonusOne, bonusPosition, Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(bonusTwo, bonusPosition, Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(bonusThree, bonusPosition, Quaternion.identity);
                        break;
                    case 4:
                        Instantiate(bonusFour, bonusPosition, Quaternion.identity);
                        break;
                }
            }

            // Put the end of the tunnel symbol
            if (index == tunnelData.Count - 1) {
                float xPosition = currentXPosition + baseSegmentLength - (baseSegmentLength / 3); // Place end of tunnel towards 2/3 of the last segment.
                float slope = (targetPosition - previousTargetPosition) / baseSegmentLength; // Find slope (y change over x change).
                float n = previousTargetPosition - (slope * currentXPosition); // Find y-intercept.
                float yPosition = (slope * xPosition) + n; // Find the corresponding y for the calculated x.

                Vector3 endTunnelPosition = new(xPosition, yPosition, 0);

                // Calculate the rotation based on the slope of the line (atan2 gives the angle in radians, converted to degrees).
                float slopeAngle = Mathf.Atan2(targetPosition - previousTargetPosition, baseSegmentLength) * Mathf.Rad2Deg;

                endTunnel.transform.localScale = new Vector3(baseSegmentLength / 12, gapSize / 2, 1);
                BoxCollider2D box = endTunnel.GetComponent<BoxCollider2D>();
                box.size = new Vector2(baseSegmentLength / 5, gapSize / 2);
                Instantiate(endTunnel, endTunnelPosition, Quaternion.Euler(0, 0, slopeAngle)); // Apply the combined rotation.
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

    private List<TunelPreporities> ReadCsv(string filePath) {
        int lineCount = -1; // because of the header
        using (var reader = new StreamReader(filePath)) {
            while (reader.ReadLine() != null) {
                lineCount++;
            }
        }

        List<TunelPreporities> list = new();
        // Create easy first segment to simplify the start of the game for three seconds.
        TunelPreporities firstRecord = new() {
            Time = 3,
            Target = 0,
            Width = 40, 
            Bonus = false
        };
        list.Add(firstRecord);

        int numOfBonus = 0;
        using  (var reader = new StreamReader(filePath)) {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture) {
                HasHeaderRecord = true // This will skip the header row
            };
            using var csv = new CsvReader(reader, config);
            csv.Read();
            csv.ReadHeader();
            float count = 0;

            while (csv.Read()) {
                count++;
                TunelPreporities proper = new() {
                    Time = csv.GetField<int>("Time"),
                    Target = csv.GetField<float>("Target"),
                    Width = csv.GetField<int>("Width")
                };

                if (proper.Target > 100 || proper.Target < -100) {
                    string message = $"In the CSV file the target in line {count} exceeds 100 or -100, when you press OK, the game " +
                                    "return to the main menu, fix the CSV file and load him again.";
                    PauseGameWithMessage(message);
                }
                if (proper.Width > 100 || proper.Width <= 15) {
                    string message = $"In the CSV file the width in line {count} exceeds 100 or smaller than 15, when you press OK, the game " +
                                    "return to the main menu, fix the CSV file and load him again.";
                    PauseGameWithMessage(message);
                }

                if (Mathf.Abs(proper.Target) + (proper.Width / 2) > 95.5f) {
                    bool negative = proper.Target < 0;

                    proper.Target = 95.5f - (Mathf.Abs(proper.Target) + proper.Width - 95.5f);
                    
                    if (negative) {
                        proper.Target *= -1;
                    }
                }

                float rand = Random.Range(0f, 1f);
                if (rand >= 0.66f && numOfBonus <= (lineCount / 3)) {
                    proper.Bonus = true;
                    numOfBonus++;
                } else {
                    proper.Bonus = false;
                }
                list.Add(proper);
            }
        }
        return list;
    }

    public void PauseGameWithMessage(string message) {

        if (messageText != null)
        {
            messageText.text = message; // Set the message text
            Debug.Log($"Setting message text to: {messageText.text}");
        }
        else
        {
            Debug.LogError("messageText reference is null!");
        }

        if (PauseObject != null)
        {
            PauseObject.SetActive(true); // Show the pause panel
            Time.timeScale = 0f; // Pause the game
        }
        else
        {
            Debug.LogError("PauseObject reference is null!");
        }
    }

    public void Resume () {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }
}

public class TunelPreporities {
    public int Time { get; set; }
    public float Target { get; set; }
    public int Width { get; set; }
    public bool Bonus { get; set; }
}