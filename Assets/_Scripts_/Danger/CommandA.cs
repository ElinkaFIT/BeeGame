using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyACommand : ICommand
{
    PlayerAI spawnReciever;
    public SpawnEnemyACommand(PlayerAI reciever)
    {
        this.spawnReciever = reciever;

    }
    public void Execute()
    {
        spawnReciever.SpawnUnit();
        Log.instance.AddNewLogText(Time.time, "New enemy", Color.black);
    }
}
