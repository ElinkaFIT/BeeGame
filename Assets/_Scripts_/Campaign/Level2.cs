//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.UI;

public class Level2 : MonoBehaviour
{
    public GameObject nextLevel;

    public Toggle taskQueen;
    public Toggle taskFood;
    public Toggle taskRest;
    public Toggle taskNewBee;
    public Toggle taskHoney;

    private bool taskQueenDone;
    private bool taskFoodDone;
    private bool taskRestDone;
    private bool taskNewBeeDone;
    private bool taskHoneyDone;

    private void Start()
    {
        taskQueenDone = false;
        taskFoodDone = false;
        taskRestDone = false;
        taskNewBeeDone = false;
        taskHoneyDone = false;

        nextLevel.SetActive(false);
    }

    void Update()
    {
        if (!taskQueenDone) { TaskQueen(); }
        if (!taskFoodDone) { TaskBuildFoodRoom(); } 
        if (!taskRestDone) { TaskBuildRestRoom(); } 
        if (!taskNewBeeDone) { TaskSpawnBee(); } 
        if (!taskHoneyDone) { TaskHoneyrPick(); }

        if (taskQueenDone && taskFoodDone && taskRestDone && taskNewBeeDone && taskHoneyDone) {
            nextLevel.SetActive(true);
        }
    }

    private void TaskQueen()
    {
        GameObject queen = GameObject.FindWithTag("Queen");

        if (queen != null)
        {
            Room queenRoom = queen.GetComponent<Room>();
            if (queenRoom.roomWorkers.Count > 0)
            {
                taskQueenDone = true;
                taskQueen.isOn = true;
            }
        }

    }

    private void TaskBuildFoodRoom()
    {
        GameObject foodRoom = GameObject.FindWithTag("FoodRoom");

        if (foodRoom != null)
        {
            taskFoodDone = true;
            taskFood.isOn = true;
        }
    }

    private void TaskBuildRestRoom()
    {
        GameObject restRoom = GameObject.FindWithTag("RestRoom");

        if (restRoom != null)
        {
            taskRestDone = true;
            taskRest.isOn = true;
        }
    }

    private void TaskSpawnBee()
    {
        GameObject nurseryObject = GameObject.FindWithTag("Nursery");

        if (nurseryObject != null)
        {
            Nursery nursery = nurseryObject.GetComponent<Nursery>();
            
            if (nursery.nurseryState == NurseryState.NewBee)
            {
                taskNewBeeDone = true;
                taskNewBee.isOn = true;
            }
        }
    }

    private void TaskHoneyrPick()
    {
        if (Hive.instance.honey >= 20)
        {
            taskHoneyDone = true;
            taskHoney.isOn = true;
        }
    }
}
