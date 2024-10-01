using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSettings : MonoBehaviour
{
    [SerializeField] private TMP_InputField speedInputField;
    [SerializeField] private Toggle[] toggles; // Array for all the toggles
    [SerializeField] private Text filePath;
    [SerializeField] private Text zeroForces;

    // Method to get the active toggle
    private string GetActiveToggle()
    {
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                return toggle.name; // Assuming each toggle has a unique name
            }
        }
        return "None";
    }

    // Method to save the data as JSON
    public void SaveSettingsToJson()
    {
        SettingsData data = new SettingsData();
        data.speed = float.Parse(speedInputField.text);
        data.filePath = filePath.text;
        data.whichFinger = GetActiveToggle();
        data.zeroForces = ConvertDictionaryToList(ConvertStringToDictionary(zeroForces.text)); // Convert dictionary to list

        Debug.Log("Settings saved eroForces: " + data.zeroForces);

        string json = JsonUtility.ToJson(data, true);

        string scriptDirectory = Application.dataPath + "/Scripts"; // Assuming your scripts are in the "Scripts" folder
        string jsonFilePath = System.IO.Path.Combine(scriptDirectory, "settings.json");

        // Save your file to this path
        System.IO.File.WriteAllText(jsonFilePath, json);


        Debug.Log("Settings saved to JSON: " + json);
    }

    public List<KeyValuePair> ConvertDictionaryToList(Dictionary<string, float> dictionary)
    {
        List<KeyValuePair> keyValuePairs = new List<KeyValuePair>();

        foreach (var kvp in dictionary)
        {
            keyValuePairs.Add(new KeyValuePair(kvp.Key, kvp.Value));
        }

        return keyValuePairs;
    }

    public Dictionary<string, float> ConvertStringToDictionary(string input)
    {
        Dictionary<string, float> dictionary = new Dictionary<string, float>();

        // Split the input string into lines
        string[] lines = input.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            // Split each line into key and value
            string[] parts = line.Split(new[] { ':' }, 2);

            if (parts.Length == 2)
            {
                string key = parts[0].Trim(); // Remove any extra whitespace
                if (float.TryParse(parts[1].Trim(), out float value)) // Try parsing the value
                {
                    dictionary[key] = value; // Add to the dictionary
                }
                else
                {
                    Debug.LogWarning($"Unable to parse value '{parts[1]}' for key '{key}'.");
                }
            }
            else
            {
                Debug.LogWarning($"Invalid line format: '{line}'.");
            }
        }

        return dictionary; // Return the populated dictionary
    }

}
