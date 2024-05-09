//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the behavior and state of a rest room within the game, controlling the resting process for workers.
/// </summary>
public class RestRoom : MonoBehaviour
{
    public RoomState state;                  // Current state of the room
    public Room curBuildRoom;                // Reference to the current building details of the room

    public int restCapacity;                 // Maximum number of workers that can rest in this room at one time
    public int restRate;                     // Rate at which workers' energy is restored

    /// <summary>
    /// Event triggered when the state of the room changes.
    /// </summary>
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<RoomState> { }
    public StateChangeEvent onStateChange;  // Event fired when the room's state changes

    /// <summary>
    /// Initializes the room by setting its state to Blueprint.
    /// </summary>
    private void Start()
    {
        SetRoomState(RoomState.Blueprint);
    }

    /// <summary>
    /// Updates the room's behavior based on its current state and handles the restRoom process.
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
    /// Handles the behavior when the room is in the Blueprint state.    /// </summary>
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
            gameObject.tag = "RestRoom";  // Update the tag to reflect the new state
        }
    }

    /// <summary>
    /// Perform resting action.    
    /// </summary>
    void BuiltUpdate()
    {
        if (curBuildRoom.roomWorkers.Count > 0)
        {
            Sleep();  // Restore energy for bee in the room
        }
    }

    /// <summary>
    /// Restores the energy of bee in the room to full capacity.
    /// </summary>
    void Sleep()
    {
        foreach (var worker in curBuildRoom.roomWorkers)
        {
            worker.curEnergy = Mathf.Min(worker.curEnergy + restRate, 100);  // Ensure energy does not exceed 100
        }
    }

    /// <summary>
    /// Checks if the rest room is full or Available
    /// </summary>
    /// <returns>True if is Available; otherwise, false.</returns>
    public bool IsRestRoomAvailable()
    {
        return restCapacity > 0 ? true : false;
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
