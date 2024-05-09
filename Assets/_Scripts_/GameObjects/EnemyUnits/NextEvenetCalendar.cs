//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the data needed to execute a command, including the type of enemy and additional data.
/// </summary>
public class CommandData
{
    public string enemyType; // Type of enemy to spawn or interact with
    public string data;      // Additional data specific to the command
}

/// <summary>
/// Manages the execution of commands based on a schedule from CommandSaver.
/// </summary>
public class NextEvenetCalendar : MonoBehaviour
{
    public CommandSaver commandSaver; // Reference to the CommandSaver that provides the next command
    public PlayerAI spawnReciever;    // Reference to the PlayerAI to execute spawn commands

    // Stack to keep track of all executed commands
    private static Stack<ICommand> finishedStack = new Stack<ICommand>();

    /// <summary>
    /// Starts the periodic execution of commands every 6 seconds after an initial delay of 2 seconds.
    /// </summary>
    private void Start()
    {
        InvokeRepeating(nameof(ExecuteCommand), 2, 6);
    }

    /// <summary>
    /// Executes the next command from the CommandSaver based on the current time.
    /// </summary>
    public void ExecuteCommand()
    {
        string commandType = commandSaver.GetNextCommand(Time.time);

        switch (commandType)
        {
            case "HiveEnemy":
                {
                    ICommand command = new SpawnEnemyBCommand(spawnReciever);
                    command.Execute();
                    finishedStack.Push(command); // Log the command execution
                    break;
                }
            case "Enemy":
                {
                    ICommand command = new SpawnEnemyACommand(spawnReciever);
                    command.Execute();
                    finishedStack.Push(command); // Log the command execution
                    break;
                }
            case "END":
                {
                    // Handle the end of the command list if needed
                    break;
                }
            default:
                {
                    // Handle any unrecognized commands
                    break;
                }
        }
    }
}
