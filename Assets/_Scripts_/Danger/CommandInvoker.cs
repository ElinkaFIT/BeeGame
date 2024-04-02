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

    private static Stack<ICommand> undoStack = new Stack<ICommand>();

    private void Start()
    {
        InvokeRepeating(nameof(ExecuteCommand), 2, 6);
    }
    public void ExecuteCommand()
    {
        string test = commandSaver.GetNextCommand(3);

        if (test == null)
        {
            return;
        }

        // tady pakl bude switch


        //command.Execute();
        //undoStack.Push(command);
    }
    //public static void UndoCommand()
    //{
    //    if (undoStack.Count > 0)
    //    {
    //        ICommand activeCommand = undoStack.Pop();
    //        activeCommand.Undo();
    //    }
    //}
}
