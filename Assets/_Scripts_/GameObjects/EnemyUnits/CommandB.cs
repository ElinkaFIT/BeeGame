//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

public class SpawnEnemyBCommand : ICommand
{
    PlayerAI spawnReciever;
    public SpawnEnemyBCommand(PlayerAI reciever)
    {
        this.spawnReciever = reciever;

    }
    public void Execute()
    {
        spawnReciever.SpawnHiveUnit();
        Log.instance.AddNewLogText(Time.time, "New enemy", Color.black);
    }
}
