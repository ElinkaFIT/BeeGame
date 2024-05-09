//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the behavior and state of a food room within the game, controlling the feeding process and construction states.
/// </summary>
public class FoodRoom : MonoBehaviour
{
    public RoomState state;                  // Current state of the room
    public Room curBuildRoom;                // Reference to the current building details of the room

    public float feedRate;                   // Rate at which the room tries to feed workers
    private float lastFeedTime;              // Time since the last feed action

    public int roomCapacity;                 // Maximum capacity of this room
    public int eatAmount;                    // Amount of feed provided to each worker per feeding session

    /// <summary>
    /// Event to trigger when the state of the room changes.
    /// </summary>
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<RoomState> { }
    public StateChangeEvent onStateChange;   // Event fired when the room's state changes

    /// <summary>
    /// Initializes the room by setting its state to Blueprint.
    /// </summary>
    private void Start()
    {
        SetRoomState(RoomState.Blueprint);
    }

    /// <summary>
    /// Updates the room's behavior based on its current state and handles the foodRoom process.
    /// </summary>
    void Update()
    {
        switch (state)
        {
            case RoomState.Blueprint:
                {
                    BlueprintUpdate();
                    break;
                }
            case RoomState.UnderConstruction:
                {
                    UnderConstructionUpdate();
                    break;
                }
            case RoomState.Built:
                {
                    BuiltUpdate();
                    break;
                }
        }
    }

    /// <summary>
    /// Handles the behavior when the room is in the Blueprint state.
    /// </summary>
    void BlueprintUpdate()
    {
        SetRoomState(RoomState.UnderConstruction);
    }

    /// <summary>
    /// Handles the behavior when the room is in the UnderConstruction state.
    /// </summary>
    void UnderConstructionUpdate()
    {
        if (curBuildRoom.concructionDone == true)
        {
            SetRoomState(RoomState.Built);
            gameObject.tag = "FoodRoom";  // Update the tag to reflect the new state
        }
    }

    /// <summary>
    /// Performs feeding actions if enough time has passed since the last feed.
    /// </summary>
    void BuiltUpdate()
    {
        // Check if enough time has passed since the feeding
        if (Time.time - lastFeedTime > feedRate)
        {
            lastFeedTime = Time.time;

            // Ensure there is enough nectar to feed bee
            bool isNectarAvailable = Hive.instance.nectar > 0;

            if (curBuildRoom.roomWorkers.Count > 0 && isNectarAvailable)
            {
                Hive.instance.RemoveMaterial(ResourceType.Nectar);

                foreach (Unit worker in curBuildRoom.roomWorkers)
                {
                    if (worker.curFeed < worker.maxFeed)
                    {
                        worker.curFeed += eatAmount;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Sets the room's state and triggers the state change event.
    /// </summary>
    /// <param name="toState">New state.</param>
    public void SetRoomState(RoomState toState)
    {
        state = toState;
        if (onStateChange != null)
        {
            onStateChange.Invoke(state);
        }
    }
}
