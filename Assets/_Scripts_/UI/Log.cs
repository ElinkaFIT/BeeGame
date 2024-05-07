//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using TMPro;
using UnityEngine;

public class Log : MonoBehaviour
{
    public static Log instance;
    public GameObject logContentPrefab;

    private float lastLogTime;
    private string lastLogName;

    void Start()
    {
        instance = this;
        lastLogTime = Time.time;
        lastLogName = "startLog";
        AddNewLogText(Time.time, "Game start", Color.black);
    }

    public void AddNewLogText(float logTime, string text, Color textColor)
    {
        if (IsItDuplicateLog(logTime, text))
        {
            return;
        }

        // generate new text
        Vector3 spawnPosition = new Vector3(0, 0, 0);
        GameObject newLog = Instantiate(logContentPrefab, spawnPosition, Quaternion.identity);

        // add text to Log panel
        newLog.transform.SetParent(gameObject.transform, false);
        //newLog.transform.parent = gameObject.transform;

        // set new text value
        TextMeshProUGUI newText = newLog.GetComponent<TextMeshProUGUI>();
        newText.color = textColor;
        int minute = ((int)logTime / 60);
        int second = ((int)logTime % 60);
        newText.text = string.Format("{0:00}:{1:00}", minute, second) + " " + text;
    }

    private bool IsItDuplicateLog(float logTime, string text)
    {
        if(text == lastLogName && logTime - lastLogTime < 1)
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
