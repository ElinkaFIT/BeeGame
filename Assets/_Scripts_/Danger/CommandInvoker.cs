using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CommandData
{
    public string enemyType;
    public string data;
}

public class CommandInvoker : MonoBehaviour
{
    public CommandSaver commandSaver;
    public PlayerAI spawnReciever;

    private static Stack<ICommand> undoStack = new Stack<ICommand>();

    private void Start()
    {
        InvokeRepeating(nameof(ExecuteCommand), 2, 6);
    }
    public void ExecuteCommand()
    {
        string commandType = commandSaver.GetNextCommand(3);

        switch (commandType)
        {
            case "HiveEnemy":
                {
                    ICommand command = new SpawnEnemyBCommand(spawnReciever);
                    command.Execute();
                    undoStack.Push(command);
                    break;
                }
            case "Enemy":
                {
                    ICommand command = new SpawnEnemyACommand(spawnReciever);
                    command.Execute();
                    undoStack.Push(command);
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
