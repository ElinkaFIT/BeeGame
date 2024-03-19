using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandA : ICommand
{
    PlayerMover playerMover;
    public CommandA(PlayerMover player)
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
