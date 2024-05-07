//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;

public class CommandData
{
    public string enemyType;
    public string data;
}

public class NextEvenetCalendar : MonoBehaviour
{
    public CommandSaver commandSaver;
    public PlayerAI spawnReciever;

    private static Stack<ICommand> finishedStack = new Stack<ICommand>();

    private void Start()
    {
        InvokeRepeating(nameof(ExecuteCommand), 2, 6);
    }
    public void ExecuteCommand()
    {
        string commandType = commandSaver.GetNextCommand(Time.time);

        switch (commandType)
        {
            case "HiveEnemy":
                {
                    ICommand command = new SpawnEnemyBCommand(spawnReciever);
                    command.Execute();
                    finishedStack.Push(command);
                    break;
                }
            case "Enemy":
                {
                    ICommand command = new SpawnEnemyACommand(spawnReciever);
                    command.Execute();
                    finishedStack.Push(command);
                    break;
                }
            case "END":
                {
                    break;
                }
            default: { break; }

        }

    }
}
