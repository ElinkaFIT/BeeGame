using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandB : ICommand
{
    PlayerMover playerMover;
    public CommandB(PlayerMover player)
    {
        this.playerMover = player;

    }
    public void Execute()
    {
        playerMover.Move();
    }
    public void Undo()
    {
        playerMover.Move();
    }
}
