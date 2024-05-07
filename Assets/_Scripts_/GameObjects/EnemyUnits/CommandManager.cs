//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;



public class CommandManager : MonoBehaviour
{
    public PlayerAI spawnReciever;
    public CommandSaver commandSaver;

    public static CommandManager instance;

    private void Start()
    {
        instance = this;
    }

    public void RunSpawnEnemy()
    {
        if (spawnReciever == null)
        {
            return;
        }
        else
        {
            Debug.Log("enemy unit");

            commandSaver.AddNewCommand(Time.time + 200, "Enemy");
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

            commandSaver.AddNewCommand(Time.time + 20, "HiveEnemy");
        }
    }
}
