//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using TMPro;
using UnityEngine;

/// <summary>
/// Manages logging events in the game, showing them in the UI.
/// </summary>
public class Log : MonoBehaviour
{
    public static Log instance;             // Singleton instance of Log.

    public GameObject logContentPrefab;     // Prefab for displaying each log entry in the UI.

    private float lastLogTime;              // Time  of the last log to prevent duplicate logs.
    private string lastLogName;             // Content of the last log to prevent duplicate logs.

    /// <summary>
    /// Initializes the Log instance and logs the game start.
    /// </summary>
    void Start()
    {
        instance = this;
        lastLogTime = Time.time;
        lastLogName = "startLog";
        AddNewLogText(Time.time, "Game start", Color.black);
    }

    /// <summary>
    /// Adds a new log entry if it's not a duplicate of the previous log.
    /// </summary>
    /// <param name="logTime">The time the log entry is created.</param>
    /// <param name="text">The log text to display.</param>
    /// <param name="textColor">The color of the log text.</param>
    public void AddNewLogText(float logTime, string text, Color textColor)
    {
        if (IsItDuplicateLog(logTime, text))
        {
            return;
        }

        // Create new log entry.
        Vector3 spawnPosition = new Vector3(0, 0, 0);
        GameObject newLog = Instantiate(logContentPrefab, spawnPosition, Quaternion.identity);

        // Add log text to log panel.
        newLog.transform.SetParent(gameObject.transform, false);

        // Set the text and color of the log entry.
        TextMeshProUGUI newText = newLog.GetComponent<TextMeshProUGUI>();
        newText.color = textColor;
        int minute = ((int)logTime / 60);
        int second = ((int)logTime % 60);
        newText.text = string.Format("{0:00}:{1:00}", minute, second) + " " + text;
    }

    /// <summary>
    /// Checks if the current log is a duplicate of the previous log.
    /// </summary>
    /// <param name="logTime">The time of the current log.</param>
    /// <param name="text">The text of the current log.</param>
    /// <returns>True if the current log is a duplicate; otherwise, false.</returns>
    private bool IsItDuplicateLog(float logTime, string text)
    {
        if (text == lastLogName && logTime - lastLogTime < 1)
        {
            lastLogTime = logTime;
            lastLogName = text;
            return true;
        }
        lastLogTime = logTime;
        lastLogName = text;
        return false;
    }
}
