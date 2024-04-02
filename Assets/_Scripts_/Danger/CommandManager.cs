using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CommandManager : MonoBehaviour
{
    public PlayerAI spawnReciever;
    public CommandSaver commandSaver;
    
    public void RunSpawnEnemy()
    {
        if (spawnReciever == null)
        {
            return;
        }
        else
        {
            Debug.Log("enemy unit");
            //ICommand command = new SpawnEnemyACommand(spawnReciever);
            //CommandInvoker.AddCommand(command, commandToJson);
            commandSaver.AddNewCommand(1, "Enemy");
        }
    }

    public void RunSpawnHiveEnemy()
    {
        if (spawnReciever == null)
        {
            return;
        }
        else
        {
            Debug.Log("enemy unit hive");
            //ICommand command = new SpawnEnemyBCommand(spawnReciever);
            //CommandInvoker.ExecuteCommand(command);
        }
    }
}
