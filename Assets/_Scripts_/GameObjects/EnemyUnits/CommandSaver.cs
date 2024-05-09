//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// CommandSaver loads pre-prepared commands from a JSON file or adds new commands dynamically.
/// </summary>
public class CommandSaver : MonoBehaviour
{
    public TextAsset jsonFile;  // contain the initial set of commands

    /// <summary>
    /// Represents a single command with its execution time and type.
    /// </summary>
    [System.Serializable]
    private class CommandObject
    {
        public float time;  // The time at which this command should execute
        public string type; // The type of command, which determines the action to be taken
    }

    /// <summary>
    /// A list of CommandObjects to be executed.
    /// </summary>
    [System.Serializable]
    private class CommandList
    {
        public List<CommandObject> commandObject; // List of all command objects
    }

    private CommandList calendar; // Holds all scheduled commands

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Initialize the command list from the provided JSON file
        if (jsonFile != null || jsonFile.text != null)
        {
            calendar = JsonUtility.FromJson<CommandList>(jsonFile.text);
        }
        CommandObject endObject = new()
        {
            time = float.PositiveInfinity,
            type = "END"
        };
        calendar.commandObject.Add(endObject);
    }

    /// <summary>
    /// Adds a new command to the list of commands with the specified time and type.
    /// </summary>
    /// <param name="time">The time at which the command should be executed.</param>
    /// <param name="type">The type of the command to be executed.</param>
    public void AddNewCommand(float time, string type)
    {
        CommandObject newObject = new()
        {
            time = time,
            type = type
        };

        calendar.commandObject.Add(newObject);
    }

    /// <summary>
    /// Retrieves and removes the next command to be executed based on the provided realTime.
    /// </summary>
    /// <param name="realTime">The current time to compare against command execution times.</param>
    /// <returns>The type of the next command to execute if it's time, otherwise null.</returns>
    public string GetNextCommand(float realTime)
    {
        // Check if the command list is empty or the first command is null
        if (calendar.commandObject == null || calendar.commandObject[0] == null)
        {
            return null;
        }

        // Find the earliest command that is scheduled to execute
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

        // If the earliest command is due, execute it and remove from the list
        if (earliest < realTime && earliestObject.type != null)
        {
            string nextCommand = earliestObject.type;
            calendar.commandObject.Remove(earliestObject);
            return nextCommand;
        }
        return null;
    }
}
