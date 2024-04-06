using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CommandManager : MonoBehaviour
{
    public PlayerAI spawnReciever;
    public CommandSaver commandSaver;
    
    public void AddSpawnEnemy()
    {
        if (spawnReciever == null)
        {
            return;
        }
        else
        {
            Debug.Log("enemy unit");

            commandSaver.AddNewCommand(1, "Enemy");
        }
    }

    public void AddSpawnHiveEnemy()
    {
        if (spawnReciever == null)
        {
            return;
        }
        else
        {
            Debug.Log("enemy unit hive");

            commandSaver.AddNewCommand(1, "HiveEnemy");
        }
    }
}
