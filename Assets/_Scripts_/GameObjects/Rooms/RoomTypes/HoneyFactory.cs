//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the honey production process within a room in the game, controlling the transition through different room states.
/// </summary>
public class HoneyFactory : MonoBehaviour
{
    public RoomState state;                // Current state of the room
    public Room curBuildRoom;              // Current building details of the room

    public float productionRate;           // Rate at which honey is produced
    private float lastProductionTime;      // Time since the last honey production

    /// <summary>
    /// Event triggered when the state of the room changes.
    /// </summary>
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<RoomState> { }
    public StateChangeEvent onStateChange; // Event to notify when the room state changes

    /// <summary>
    /// Initializes the room by setting its state to Blueprint.
    /// </summary>
    private void Start()
    {
        SetRoomState(RoomState.Blueprint);
    }

    /// <summary>
    /// Updates the room's behavior based on its current state and handles the honey production process.
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
            gameObject.tag = "HoneyFactory";
        }
    }

    /// <summary>
    /// Perform the production of honey based on the availability of nectar and the presence of workers.
    /// </summary>
    void BuiltUpdate()
    {
        // Check if enough time has passed since the last production
        if (Time.time - lastProductionTime > productionRate)
        {
            lastProductionTime = Time.time;

            // Ensure there is enough nectar to produce honey
            bool isNectarAvailable = Hive.instance.nectar > 3;

            if (curBuildRoom.roomWorkers.Count > 0 && isNectarAvailable)
            {
                // Consume three times nectar to produce honey
                Hive.instance.RemoveMaterial(ResourceType.Nectar);
                Hive.instance.RemoveMaterial(ResourceType.Nectar);
                Hive.instance.RemoveMaterial(ResourceType.Nectar);
                Hive.instance.GainResource(ResourceType.Honey, 1);
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
            onStateChange.Invoke(state);
    }
}
}
