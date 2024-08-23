using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;

public class tunelSpawner : MonoBehaviour
{
    [SerializeField] int time;
    [SerializeField] int target;
    [SerializeField] int width;
    [SerializeField] int time_of_the_game;
    List<tunelPreporities> preporities;

    void Start()
    {
        string path = @"C:\unityProjects\theBird\finalProject\Assets\tunelProperities.csv";
        preporities = ReadCsv(path);

        time = preporities[0].Time;
        target = preporities[0].Target;
        width = preporities[0].Width;
    }

    // Update is called once per frame
    void Update()
    {
        var start_time = System.DateTime.Now;
        int interval = 1;
        while ((System.DateTime.Now - start_time).TotalSeconds < time_of_the_game) {
            var start_time_for_current_properities = System.DateTime.Now;
            bool rondel = true;
            while ((System.DateTime.Now - start_time_for_current_properities).TotalSeconds < time) {
                if (rondel) {
                    Debug.Log($"time: {time}, target: {target}, width: {width}");
                    rondel = false;
                }
            }
            time = preporities[interval].Time;
            target = preporities[interval].Target;
            width = preporities[interval].Width;
            interval++;
            if (interval == preporities.Count - 1) {
                break;
            }
            rondel = true;
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