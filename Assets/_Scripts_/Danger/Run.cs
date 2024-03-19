using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public PlayerMover playerMover;
    private void Start()
    {
        RunPlayerCommand(playerMover);

        RunPlayerCommand(playerMover);

        RunPlayerCommand(playerMover);
    }
    private void RunPlayerCommand(PlayerMover playerMover)
    {
        if (playerMover == null)
        {
            return;
        }
        else
        {
            ICommand command = new CommandA(playerMover);
            CommandInvoker.ExecuteCommand(command);

            ICommand commandB = new CommandB(playerMover);
            CommandInvoker.ExecuteCommand(commandB);
        }
    }
}
