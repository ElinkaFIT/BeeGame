//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the possible states of a room within the game.
/// </summary>
public enum RoomState
{
    Blueprint,           // Room is in the blueprint phase and is not yet being built.
    UnderConstruction,   // Room is currently under construction.
    Built                // Room has been fully constructed.
}

/// <summary>
/// Manages the attributes and behaviors of a room, including its construction, health, and worker management.
/// </summary>
public class Room : MonoBehaviour
{
    public RoomPreset preset;                   // Preset configuration for the room
    public BuildProgressBar progressBar;        // UI component showing the construction progress
    public GameObject doneIcon;                 // Icon displayed when the room is completely built
    public GameObject blueprint;                // Blueprint graphic shown before construction starts

    public int buildProgress;                   // Current progress of the room's construction
    public int buildModifier;                   // Additional modifier to speed up construction

    public bool concructionDone;                // Flag indicating if construction is complete

    public int curRoomHealth;                   // Current health of the room

    public List<Unit> roomWorkers;              // List of worker units currently assigned to this room

    /// <summary>
    /// Initializes the room, setting initial health and construction state.
    /// </summary>
    private void Start()
    {
        curRoomHealth = preset.roomHealthMax;   // Set the room's health to its maximum based on the preset
        concructionDone = false;                // Construction is not done initially
        doneIcon.SetActive(false);              // Hide the done icon until construction is complete
    }

    /// <summary>
    /// Advances the construction progress of the room and updates the UI.
    /// </summary>
    /// <param name="amount">The amount to increase the build progress by.</param>
    public void BuildRoom(int amount)
    {
        if (buildProgress >= 100)
        {
            concructionDone = true;                
            progressBar.CloseProgressBar();         
            blueprint.SetActive(false);             
            // deleteRoomButton.SetActive(false);  
            doneIcon.SetActive(true);              
            return;
        }

        buildProgress = buildProgress + buildModifier + amount;
        progressBar.UpdateProgressBar(buildProgress, 100);
    }

    /// <summary>
    /// Applies damage to the room, potentially destroying it if health drops to zero or below.
    /// </summary>
    /// <param name="amount">The amount of damage to apply to the room.</param>
    public void TakeRoomDmg(int amount)
    {
        curRoomHealth -= amount;  

        if (curRoomHealth <= 0)
        {
            Hive.instance.rooms.Remove(this);  
            Destroy(gameObject);               
            return;
        }
    }

    /// <summary>
    /// Assigns a worker to this room, adding them to the roomWorkers list.
    /// </summary>
    /// <param name="newWorker">The worker unit to be added to the room.</param>
    public void WorkInRoom(Unit newWorker)
    {
        if (!roomWorkers.Contains(newWorker))
        {
            roomWorkers.Add(newWorker); 
            doneIcon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);  
        }
    }

    /// <summary>
    /// Removes a worker from this room, taking them off the roomWorkers list.
    /// </summary>
    /// <param name="newWorker">The worker unit to be removed from the room.</param>
    public void StopWorkInRoom(Unit newWorker)
    {
        if (roomWorkers.Contains(newWorker))
        {
            roomWorkers.Remove(newWorker); 
            doneIcon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f); 
        }
    }

    /// <summary>
    /// Deletes the room from the hive.
    /// </summary>
    public void DeleteRoom()
    {
        Hive.instance.OnRemoveBuilding(this);  // Notify the hive that a building is being removed
    }
}
