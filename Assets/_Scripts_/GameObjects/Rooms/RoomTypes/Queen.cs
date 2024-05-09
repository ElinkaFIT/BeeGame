//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Define different states for the queen bee
public enum QueenState
{
    Normal,         // Queen is in a normal state
    Hungry,         // Queen is hungry and needs more food
    QueenIsDying,   // Queen is dying and close to death
    QueenDeath      // Queen is dead
}

public class Queen : MonoBehaviour
{
    public RoomState state;                                     // Current state of the room
    public QueenState queenState;                               // Current state of the queen
    public Room curBuildRoom;                                   // Current room the queen is building

    /// <summary>
    /// Event triggered when the state of the room changes.
    /// </summary>
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<RoomState> { }
    public StateChangeEvent onStateChange; // Event fired when the room's state changes

    public float queenConsumption;                              // Current amount of food the queen has consumed
    public float consumptionRate;                               // Rate at which the queen consumes food
    private float lastConsumptionTime;                          // Time since the queen last consumed food

    public int lowConsumptionLimit;                             // Lower limit at which the queen starts to get hungry
    public int criticalConsumptionLimit;                        // Critical limit at which the queen starts dying

    public float LayingEggRate;                                 // Rate at which the queen lays eggs
    private float lastLayingEggTime;                            // Time since the queen last laid eggs

    /// <summary>
    /// Initializes the room by setting its state to Blueprint.
    /// </summary>
    private void Start()
    {
        SetRoomState(RoomState.Blueprint);
    }

    /// <summary>
    /// Updates the room's behavior based on its current state
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
        curBuildRoom.BuildRoom(100);
        if (curBuildRoom.concructionDone == true)
        {
            SetRoomState(RoomState.Built);
            gameObject.tag = "Queen";
        }
    }

    /// <summary>
    /// Manages the queen operations when the room is fully built and operational.
    /// </summary>
    void BuiltUpdate()
    {
        switch (queenState)
        {
            case QueenState.Normal:
                {
                    NormalQueenUpdate();
                    break;
                }
            case QueenState.Hungry:
                {
                    HungryQueenUpdate();
                    break;
                }
            case QueenState.QueenIsDying:
                {
                    QueenIsDyingUpdate();
                    break;
                }
            case QueenState.QueenDeath:
                {
                    QueenDeathUpdate();
                    break;
                }
        }
    }

    /// <summary>
    /// Manage the queen's consumption of resources.
    /// </summary>
    void QueenConsumption()
    {
        // Check if enough time has passed since the queen feeding
        if (Time.time - lastConsumptionTime > consumptionRate)
        {
            lastConsumptionTime = Time.time;

            bool isPollenAvailable = Hive.instance.pollen > 0;
            bool isNectarAvailable = Hive.instance.nectar > 0;

            // Feed the queen if resources are available and the queen is not fully fed
            if (curBuildRoom.roomWorkers.Count > 0 && isPollenAvailable && isNectarAvailable && queenConsumption < 100)
            {
                Hive.instance.RemoveMaterial(ResourceType.Nectar);
                Hive.instance.RemoveMaterial(ResourceType.Pollen);
                queenConsumption += 1;
            }
            else
            {
                queenConsumption -= 1;
            }
        }
    }

    /// <summary>
    /// Select a nursery room for the queen to lay eggs.
    /// </summary>
    /// <returns>Selected nursery room or null if none found.</returns>
    private Room PickingNursery()
    {
        List<Room> rooms = Hive.instance.rooms;
        List<Room> nurseryRooms = new List<Room>();

        // Select only empty and built nursery rooms
        foreach (Room room in rooms)
        {
            if (room.preset.roomType == RoomType.Nursery && room.concructionDone)
            {
                GameObject gameObject = room.gameObject;
                if (gameObject.GetComponent<Nursery>().nurseryState == NurseryState.Empty)
                {
                    nurseryRooms.Add(room);
                }
            }
        }

        // Choose a suitable nursery
        foreach (Room nursery in nurseryRooms)
        {
            if (nursery.roomWorkers.Count > 0)
            {
                return nursery;
            }
        }

        if (nurseryRooms.Count > 0)
        {
            return nurseryRooms[0];
        }
        return null;
    }

    /// <summary>
    /// Manage the queen's egg-laying process.
    /// </summary>
    void QueenLayingEggs()
    {
        // Extend the laying cycle based on how hungry the queen is
        float delay = 50 - queenConsumption;

        // Lay eggs
        if (Time.time - lastLayingEggTime > LayingEggRate + delay)
        {
            lastLayingEggTime = Time.time;

            Room pickedRoom = PickingNursery();
            if (pickedRoom != null)
            {
                Log.instance.AddNewLogText(Time.time, "Queen lay a new egg", Color.black);
                pickedRoom.gameObject.GetComponent<Nursery>().AddNewEgg();

                if (Random.Range(0f, 1f) < 1f)
                {
                    CommandManager.instance.RunSpawnEnemy();
                }
            }
        }
    }

    /// <summary>
    /// Update function for when the queen is in 'Normal' state.
    /// </summary>
    void NormalQueenUpdate()
    {
        QueenConsumption();
        QueenLayingEggs();

        // Change state if the queen reaches the low consumption threshold
        if (queenConsumption <= lowConsumptionLimit)
        {
            Log.instance.AddNewLogText(Time.time, "Queen is hungry", Color.red);
            SetQueenState(QueenState.Hungry);
        }
    }

    /// <summary>
    /// Update function for when the queen is 'Hungry'.
    /// </summary>
    void HungryQueenUpdate()
    {
        QueenConsumption();

        // Change the state based on the consumption levels
        if (queenConsumption <= criticalConsumptionLimit)
        {
            Log.instance.AddNewLogText(Time.time, "Queen is starving", Color.red);
            SetQueenState(QueenState.QueenIsDying);
        }
        else if (queenConsumption > lowConsumptionLimit)
        {
            SetQueenState(QueenState.Normal);
        }
    }

    /// <summary>
    /// Update function for when the queen is 'Dying'.
    /// </summary>
    void QueenIsDyingUpdate()
    {
        QueenConsumption();

        // Flashing UI component to indicate the game is near end
        UIComponents.instance.StartGameOverReminder();

        // Change state if the queen reaches the death threshold
        if (queenConsumption <= 0)
        {
            SetQueenState(QueenState.QueenDeath);
            UIComponents.instance.StopGameOverReminder();
        }
        else if (queenConsumption > criticalConsumptionLimit)
        {
            SetQueenState(QueenState.Hungry);
            UIComponents.instance.StopGameOverReminder();
        }
    }

    /// <summary>
    /// Update function for when the queen is 'Dead'.
    /// </summary>
    void QueenDeathUpdate()
    {
        // Trigger the game over menu
        GameOver.instance.OpenGameOverMenu();
    }

    /// <summary>
    /// Set the state of the room and trigger the state change event.
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

    /// <summary>
    /// Set the state of the queen and trigger the state change event.
    /// </summary>
    /// <param name="toState">New state.</param>
    public void SetQueenState(QueenState toState)
    {
        queenState = toState;

        if (onStateChange != null)
        {
            onStateChange.Invoke(state);
        }
    }
}
