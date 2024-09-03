using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;

public class tunelSpawner : MonoBehaviour
{
    public GameObject wallPrefab;  
    public float characterSpeed;  
    public float screenHeight;   
    [SerializeField] int time_of_the_game;
    List<tunelPreporities> preporities;

    void Start()
    {
        string path = @"C:\unityProjects\theBird\finalProject\Assets\tunelProperities.csv";
        preporities = ReadCsv(path);
        GenerateTunnel(preporities);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateTunnel(List<tunelPreporities> tunnelData)
    {
       
        float currentXPosition = 0;   // Start the tunnel from the origin on the x-axis
        float previousTargetPosition = screenHeight / 2;  // Start at the center of the screen

        for (int index = 0; index < tunnelData.Count; index++)
        {
            var entry = tunnelData[index];
            float time = entry.Time;         // Time for the character to pass this segment
            float target = entry.Target;     // Target position (percentage of screen height)
            float width = entry.Width;       // Width of the tunnel gap (percentage of screen height)

            // Calculate the actual target position
            float targetPosition = target / 100.0f * screenHeight;
            float gapSize = width / 100.0f * screenHeight;
            float halfGap = gapSize / 2.0f;

            // Calculate the length of this tunnel segment
            float segmentLength = characterSpeed * time;

            // Determine the starting Y position for the first segment
            float startYPosition = index == 0 ? targetPosition : previousTargetPosition;

            // Calculate the vertical shift per unit of length
            float verticalShiftPerUnit = (targetPosition - startYPosition) / segmentLength;

            // Create tunnel segments across the length
            for (float x = 0; x < segmentLength; x += 0.1f) // Adjust step size for smoothness
            {
                float currentYPosition = startYPosition + verticalShiftPerUnit * x;

                // Create the bottom wall
                Vector3 bottomWallPosition = new Vector3(currentXPosition + x, currentYPosition - halfGap, 0);
                GameObject bottomWall = Instantiate(wallPrefab, bottomWallPosition, Quaternion.identity);
                bottomWall.transform.localScale = new Vector3(0.1f, 0.1f, 1);  // Adjust thickness if necessary

                // Create the top wall
                Vector3 topWallPosition = new Vector3(currentXPosition + x, currentYPosition + halfGap, 0);
                GameObject topWall = Instantiate(wallPrefab, topWallPosition, Quaternion.identity);
                topWall.transform.localScale = new Vector3(0.1f, 0.1f, 1);  // Adjust thickness if necessary
            }

            // Move to the next segment's start position
            currentXPosition += segmentLength;  // Move forward by the length of this segment

            // Update the previous target position
            previousTargetPosition = targetPosition;
        }
    }

    public static List<tunelPreporities> ReadCsv(string filePath)
    {
        using (var reader = new StreamReader(filePath))
        using (var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            var records = csv.GetRecords<tunelPreporities>();
            return new List<tunelPreporities>(records);
        }
    }
}

public class tunelPreporities {
    public int Time { get; set; }
    public int Target { get; set; }
    public int Width { get; set; }
}