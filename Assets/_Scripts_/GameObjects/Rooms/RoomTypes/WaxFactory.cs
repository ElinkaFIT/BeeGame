//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the wax production within the game.
/// </summary>
public class WaxFactory : MonoBehaviour
{
    public RoomState state;                  // Current state of the room
    public Room curBuildRoom;                // Reference to the current building details of the room

    public float productionRate;             // Rate at which wax is produced
    private float lastProductionTime;        // Time since the last production of wax

    /// <summary>
    /// Event triggered when the state of the room changes.
    /// </summary>
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<RoomState> { }
    public StateChangeEvent onStateChange; // Event fired when the room's state changes

    /// <summary>
    /// Initializes the room by setting its state to Blueprint.
    /// </summary>
    private void Start()
    {
        SetRoomState(RoomState.Blueprint);
    }

    /// <summary>
    /// Updates the room's behavior based on its current state and handles the WaxFactory process.
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
            gameObject.tag = "WaxFactory";  // Update the tag to reflect the new state
        }
    }

    /// <summary>
    /// Performs Wax production if enough time has passed since the last production.
    /// </summary>
    void BuiltUpdate()
    {
        // Check if enough time has passed since last production
        if (Time.time - lastProductionTime > productionRate)
        {
            lastProductionTime = Time.time;

            // Produce wax if there is bee in the factory
            if (curBuildRoom.roomWorkers.Count > 0)
            {
                Hive.instance.GainResource(ResourceType.Wax, 1);
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
