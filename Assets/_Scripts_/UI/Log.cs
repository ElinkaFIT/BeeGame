using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Log : MonoBehaviour
{
    public static Log instance;
    public GameObject logContentPrefab;

    void Start()
    {
        instance = this;
        AddNewLogText(Time.time, "Game start", Color.black);
    }

    public void AddNewLogText(float logTime, string text, Color textColor)
    {
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
    
}
