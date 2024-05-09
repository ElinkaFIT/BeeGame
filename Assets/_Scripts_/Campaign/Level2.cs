//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Level2 class manages the tasks within the second level of the game.
/// It checks for the completion of specific tasks and enables progression to the next level.
/// </summary>
public class Level2 : MonoBehaviour
{
    public GameObject nextLevel;             // Object for moving to the next level

    // UI toggles to display task completion
    public Toggle taskQueen;                 // Toggle for the Queen task
    public Toggle taskFood;                  // Toggle for the Food Room task
    public Toggle taskRest;                  // Toggle for the Rest Room task
    public Toggle taskNewBee;                // Toggle for the New Bee task
    public Toggle taskHoney;                 // Toggle for the Honey task

    // Variables to track if a task is completed
    private bool taskQueenDone;              // Flag for Queen task completion
    private bool taskFoodDone;               // Flag for Food Room task completion
    private bool taskRestDone;               // Flag for Rest Room task completion
    private bool taskNewBeeDone;             // Flag for New Bee task completion
    private bool taskHoneyDone;              // Flag for Honey task completion

    /// <summary>
    /// Initialize tasks as incomplete and set the next level object to inactive.
    /// </summary>
    private void Start()
    {
        taskQueenDone = false;              // Initialize the queen task as incomplete
        taskFoodDone = false;               // Initialize the food room task as incomplete
        taskRestDone = false;               // Initialize the rest room task as incomplete
        taskNewBeeDone = false;             // Initialize the new bee task as incomplete
        taskHoneyDone = false;              // Initialize the honey task as incomplete

        nextLevel.SetActive(false);          // Initially, do not show the next level object
    }

    /// <summary>
    /// Update is called once per frame to check task completion.
    /// </summary>
    void Update()
    {
        if (!taskQueenDone) { TaskQueen(); }
        if (!taskFoodDone) { TaskBuildFoodRoom(); }
        if (!taskRestDone) { TaskBuildRestRoom(); }
        if (!taskNewBeeDone) { TaskSpawnBee(); }
        if (!taskHoneyDone) { TaskHoneyrPick(); }

        if (taskQueenDone && taskFoodDone && taskRestDone && taskNewBeeDone && taskHoneyDone)
        {
            nextLevel.SetActive(true);        // Activate the next level object if all tasks are completed
        }
    }

    /// <summary>
    /// Checks if the bee is assigned to a queen to complete the queen task.
    /// </summary>
    private void TaskQueen()
    {
        GameObject queen = GameObject.FindWithTag("Queen");

        if (queen != null)
        {
            Room queenRoom = queen.GetComponent<Room>();
            if (queenRoom != null && queenRoom.roomWorkers.Count > 0)
            {
                taskQueenDone = true;
                taskQueen.isOn = true;        // Update the UI toggle
            }
        }
    }

    /// <summary>
    /// Checks if a food room has been built to complete the food room task.
    /// </summary>
    private void TaskBuildFoodRoom()
    {
        GameObject foodRoom = GameObject.FindWithTag("FoodRoom");

        if (foodRoom != null)
        {
            taskFoodDone = true;
            taskFood.isOn = true;             // Update the UI toggle
        }
    }

    /// <summary>
    /// Checks if a rest room has been built to complete the restroom task.
    /// </summary>
    private void TaskBuildRestRoom()
    {
        GameObject restRoom = GameObject.FindWithTag("RestRoom");

        if (restRoom != null)
        {
            taskRestDone = true;
            taskRest.isOn = true;             // Update the UI toggle
        }
    }

    /// <summary>
    /// Checks if a new bee has been spawned in the nursery to complete the new bee task.
    /// </summary>
    private void TaskSpawnBee()
    {
        GameObject nurseryObject = GameObject.FindWithTag("Nursery");

        if (nurseryObject != null)
        {
            Nursery nursery = nurseryObject.GetComponent<Nursery>();

            if (nursery != null && nursery.nurseryState == NurseryState.NewBee)
            {
                taskNewBeeDone = true;
                taskNewBee.isOn = true;       // Update the UI toggle
            }
        }
    }

    /// <summary>
    /// Checks if the honey collected is sufficient to complete the honey task.
    /// </summary>
    private void TaskHoneyrPick()
    {
        if (Hive.instance.honey >= 20)
        {
            taskHoneyDone = true;
            taskHoney.isOn = true;            // Update the UI toggle
        }
    }
}
