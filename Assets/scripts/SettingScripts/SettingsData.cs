using System.Collections.Generic;
[System.Serializable]
public class SettingsData
{
    public float speed;
    public string filePath;
    public string whichFinger;
    public List<KeyValuePair> zeroForces; // Store key-value pairs here
}

[System.Serializable]
public class KeyValuePair
{
    public string Key; // This will hold the dictionary key
    public float Value; // This will hold the dictionary value

    public KeyValuePair(string key, float value)
    {
        Key = key;
        Value = value;
    }
}
