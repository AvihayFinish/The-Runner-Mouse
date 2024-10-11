using UnityEngine;
using UnityEngine.UI;
using SFB;
using System.IO; // Add this to work with files

public class FileExplorer : MonoBehaviour
{
    public Text filePathText; // A Text UI to display the selected file path
    private string filePath;

    // Method to open the file explorer and select a CSV file
    public void OpenFileExplorer()
    {

        // Open the file dialog to select the CSV file (this works in a standalone build)
        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select CSV file", "", "csv", false);

        // If a file was selected, load the CSV and generate the tunnel
        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0])) {
            filePath = paths[0]; // Save the file path
            filePathText.text = filePath; // Display the file path in the UI
            Debug.Log("Selected file path: " + filePath);
        } else {
            Debug.LogWarning("No file selected.");
        }
    }

    public string GetSelectedFilePath()
    {
        return filePath; // Return the selected file path if needed elsewhere
    }
}
