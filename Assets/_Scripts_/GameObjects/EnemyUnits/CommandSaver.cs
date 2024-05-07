//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;


// nacte predpripravene commandy, nebo k nim prida novy
public class CommandSaver : MonoBehaviour
{
    public TextAsset jsonFile;

    [System.Serializable]
    private class CommandObject
    {
        public float time;
        public string type;
    }

    [System.Serializable]
    private class CommandList
    {
        public List<CommandObject> commandObject;
    }

    private CommandList calendar;

    private void Awake()
    {
        // naète data pro potøebnou kampaò
        if (jsonFile != null || jsonFile.text != null) {
            calendar = JsonUtility.FromJson<CommandList>(jsonFile.text);
        }
        CommandObject endObject = new()
        {
            time = float.PositiveInfinity,
            type = "END"
        };
        calendar.commandObject.Add(endObject);
    }

    public void AddNewCommand(float time, string type)
    {
        CommandObject newObject = new()
        {
            time = time,
            type = type
        };

        calendar.commandObject.Add(newObject);
    }

    public string GetNextCommand(float realTime)
    {
        if (calendar.commandObject == null || calendar.commandObject[0] == null)
        {
            return null;
        }

        float earliest = calendar.commandObject[0].time;
        CommandObject earliestObject = calendar.commandObject[0];

        foreach (var commandObject in calendar.commandObject)
        {
            if (commandObject != null && commandObject.time < earliest)
            {
                earliest = commandObject.time;
                earliestObject = commandObject;
            }
        }
        if (earliest < realTime && earliestObject.type != null)
        {
            string nextCommand = earliestObject.type;

            calendar.commandObject.Remove(earliestObject);

            return nextCommand;

        }
        return null;
    }
}
