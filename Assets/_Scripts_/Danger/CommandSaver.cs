using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

// nacte predpripravene commandy, nebo k nim prida novy
public class CommandSaver : MonoBehaviour
{
    public TextAsset jsonFile;

    [System.Serializable]
    public class CommandObject
    {
        public int time;
        public string type;
    }

    [System.Serializable]
    public class CommandList
    {
        public List<CommandObject> commandObject;
    }

    public CommandList calendar;

    private void Awake()
    {

        // naète data pro potøebnou kampaò
        if (jsonFile != null || jsonFile.text != null) {
            calendar = JsonUtility.FromJson<CommandList>(jsonFile.text);
        }
        CommandObject endObject = new()
        {
            time = 5000,
            type = "END"
        };
        calendar.commandObject.Add(endObject);
    }

    public void AddNewCommand(int time, string type)
    {
        CommandObject newObject = new()
        {
            time = time,
            type = type
        };

        calendar.commandObject.Add(newObject);
    }

    public string GetNextCommand(int realTime)
    {
        if (calendar.commandObject[0] == null || calendar.commandObject == null)
        {
            return null;
        }

        int earliest = calendar.commandObject[0].time;
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
