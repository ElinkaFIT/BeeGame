//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Level1 class manages the tasks within the first level of the game.
/// It checks for the completion of specific tasks and enables progression to the next level.
/// </summary>
public class Level1 : MonoBehaviour
{
    public GameObject nextLevel;             // Object for moving to the next level

    // UI toggles to display task completion
    public Toggle taskHive;                  // Toggle for the Hive task
    public Toggle taskUnit;                  // Toggle for the Unit task
    public Toggle taskSelect;                // Toggle for the Select task
    public Toggle taskWater;                 // Toggle for the Water task
    public Toggle taskNectar;                // Toggle for the Nectar task

    // Variables to track if a task is completed
    private bool taskHiveDone;               // Flag for Hive task completion
    private bool taskUnitDone;               // Flag for Unit task completion
    private bool taskSelectDone;             // Flag for Select task completion
    private bool taskWaterDone;              // Flag for Water task completion
    private bool taskNectarDone;             // Flag for Nectar task completion

    /// <summary>
    /// Initialize tasks as incomplete and set the next level object to inactive.
    /// Set the queen to immortal status after a short delay.
    /// </summary>
    private void Start()
    {
        taskHiveDone = false;               // Initialize the hive task as incomplete
        taskUnitDone = false;               // Initialize the unit task as incomplete
        taskWaterDone = false;              // Initialize the water task as incomplete
        taskNectarDone = false;             // Initialize the nectar task as incomplete
        taskSelectDone = false;             // Initialize the select task as incomplete

        nextLevel.SetActive(false);          // Initially, do not show the next level object

        Invoke(nameof(SetImmortalQueen), 0.1f);
    }

    /// <summary>
    /// Sets the queen's consumption rate to infinite, making her immortal.
    /// </summary>
    void SetImmortalQueen()
    {
        GameObject queen = GameObject.FindWithTag("Queen"); // Find the queen object by tag
        if (queen != null)
        {
            Queen queenScript = queen.GetComponent<Queen>();
            queenScript.queenConsumption = float.PositiveInfinity; // Make queen immortal
        }
    }

    /// <summary>
    /// Update is called once per frame to check task completion.
    /// If all tasks are done, activate the next level object.
    /// </summary>
    void Update()
    {
        if (!taskHiveDone) { TaskHiveMove(); }
        if (!taskUnitDone) { TaskUnitMove(); }
        if (!taskWaterDone) { TaskWaterPick(); }
        if (!taskNectarDone) { TaskNectarPick(); }
        if (!taskSelectDone) { TaskSelectUnits(); }

        if (taskHiveDone && taskUnitDone && taskSelectDone && taskNectarDone && taskWaterDone)
        {
            nextLevel.SetActive(true);        // Activate the next level object if all tasks are completed
        }
    }

    /// <summary>
    /// Checks if the camera is at the hive's location to complete the hive task.
    /// </summary>
    private void TaskHiveMove()
    {
        Vector3 cameraPosition = Camera.main.transform.position;

        // If the camera is at the hive location
        if (cameraPosition.x > -10 && (-10 < cameraPosition.y && cameraPosition.y < 30))
        {
            taskHiveDone = true;
            taskHive.isOn = true;            // Update the UI toggle
        }
    }

    /// <summary>
    /// Checks if any unit is close enough to any empty room in the hive to complete the unit task.
    /// </summary>
    private void TaskUnitMove()
    {
        // This loop iterates over all player units
        foreach (Unit unit in Player.me.units)
        {
            // This loop iterates over all empty rooms in the hive
            foreach (GameObject room in HiveGenerator.instance.emptyRooms)
            {
                Vector2 roomPos = room.transform.position;
                Vector2 unitPos = unit.transform.position;

                // Check if the unit is within a proximity threshold to an empty room
                if (Vector2.Distance(roomPos, unitPos) < 1) // Check proximity
                {
                    taskUnitDone = true;
                    taskUnit.isOn = true;        // Update the UI toggle
                }
            }
        }
    }

    /// <summary>
    /// Checks if all units are selected to complete the select units task.
    /// This compares the number of selected units to the total number of player units.
    /// </summary>
    private void TaskSelectUnits()
    {
        if (UnitSelection.instance.selectedUnits.Count == Player.me.units.Count)
        {
            taskSelectDone = true;
            taskSelect.isOn = true;            // Update the UI toggle
        }
    }

    /// <summary>
    /// Checks if the water collected is sufficient to complete the water task.
    /// This is based on the water quantity in the hive instance.
    /// </summary>
    private void TaskWaterPick()
    {
        if (Hive.instance.water >= 20)
        {
            taskWaterDone = true;
            taskWater.isOn = true;             // Update the UI toggle
        }
    }

    /// <summary>
    /// Checks if the nectar collected is sufficient to complete the nectar task.
    /// This is based on the nectar quantity in the hive instance.
    /// </summary>
    private void TaskNectarPick()
    {
        if (Hive.instance.nectar >= 20)
        {
            taskNectarDone = true;
            taskNectar.isOn = true;            // Update the UI toggle
        }
    }
}
