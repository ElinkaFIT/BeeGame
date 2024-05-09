//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the warehouse functionality within the game, handling the increase in storage capacity upon completion.
/// </summary>
public class Warehouse : MonoBehaviour
{
    public RoomState state;                  // Current state of the room
    public Room curBuildRoom;                // Reference to the current building details of the room
    public int addingCapacity;               // Amount of capacity to add to storage when the warehouse is built

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
    /// Updates the room's behavior based on its current state.
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
    /// Increases the capacity of resources upon completion.
    /// </summary>
    void UnderConstructionUpdate()
    {
        if (curBuildRoom.concructionDone == true)
        {
            IncreaseCapacity();  // Increase the maximum storage capacity
            SetRoomState(RoomState.Built);
        }
    }

    /// <summary>
    /// Increases the storage capacity for resources in the Hive.
    /// </summary>
    private void IncreaseCapacity()
    {
        // Increase nectar capacity and update the UI
        int newNectarCapacity = Hive.instance.nectarCapacity + addingCapacity;
        Hive.instance.nectarCapacity = newNectarCapacity;
        GameUI.instance.UpdateNectarCapacity(newNectarCapacity);

        // Increase water capacity and update the UI
        int newWaterCapacity = Hive.instance.waterCapacity + addingCapacity;
        Hive.instance.waterCapacity = newWaterCapacity;
        GameUI.instance.UpdateWaterCapacity(newWaterCapacity);
    }

    /// <summary>
    /// Sets the room's state and triggers the state change event.
    /// </summary>
    /// <param name="toState">The new state.</param>
    public void SetRoomState(RoomState toState)
    {
        state = toState;
        if (onStateChange != null)
        {
            onStateChange.Invoke(state);
        }
    }
}
