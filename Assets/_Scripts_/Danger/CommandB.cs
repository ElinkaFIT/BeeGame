using System.Collections;
using System.Collections.Generic;
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
    }
    //public void Undo()
    //{
    //    spawnReciever.Spawn();
    //}
}
