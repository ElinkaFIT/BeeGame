//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages the states and behaviors of a nursery room, facilitating the growth and emergence of new bees.
/// </summary>
public enum NurseryState
{
    Empty,          // The nursery is empty and ready to receive an egg.
    BeeGrows,       // A bee larva is growing in the nursery.
    WaitingForLive, // The larva has pupated and is waiting to emerge as a bee.
    NewBee          // A new bee is ready to emerge from the nursery.
}

/// <summary>
/// Manages the behavior and state of a Nursery within the game, controlling the larva growing and construction states.
/// </summary>
public class Nursery : MonoBehaviour
{
    public RoomState state;                  // Current state of the room
    public Room curBuildRoom;                // Reference to the current build room.
    public NurseryState nurseryState;        // Current state of the nursery related to bee development.

    private float timeOfStartGrow;           // Time when a bee larva started growing.
    public float timeToGrow;                 // Time required for a bee larva to grow before pupating.

    private float timeOfStartPupation;       // Time when a larva started pupating.
    public float timeToSpawn;                // Time required for a pupa to develop into a bee.

    public int newBeeConsumption;            // Amount of resources consumed by the new bee during its development.
    public float consumptionRate;            // Rate at which the new bee consumes resources.
    private float lastConsumptionTime;       // Time since the last consumption by the bee.

    public int lowConsumptionLimit;          // Threshold below which the larva is considered to be starving.

    private int careIdentifire;              // Identifier used for calculating care provided to the bee.
    private int startBeeConsumption;         // Initial amount of resources consumed by the new bee.

    public GameObject beePrefab;             // Prefab used for creating a new bee.

    /// <summary>
    /// Event triggered when the state of the room changes.
    /// </summary>
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<RoomState> { }
    public StateChangeEvent onStateChange;  // Event fired when the room's state changes

    /// <summary>
    /// Sets initial conditions and states at the start.
    /// </summary>
    private void Start()
    {
        startBeeConsumption = newBeeConsumption; 
        SetRoomState(RoomState.Blueprint);        
    }

    /// <summary>
    /// Updates the nursery behavior based on its current state.
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
            gameObject.tag = "Nursery"; 
        }
    }

    /// <summary>
    /// Manages the nursery operations when the room is fully built and operational.
    /// </summary>
    void BuiltUpdate()
    {
        switch (nurseryState)
        {
            case NurseryState.Empty:
                {
                    break;
                }
            case NurseryState.BeeGrows:
                {
                    BeeGrowsUpdate();
                    break;
                }
            case NurseryState.WaitingForLive:
                {
                    WaitingForLiveUpdate();
                    break;
                }
            case NurseryState.NewBee:
                {
                    NewBeeUpdate();
                    break;
                }

        }
    }

    /// <summary>
    /// Manages the consumption of resources by a growing bee larva.
    /// </summary>
    void NewBeeConsumption()
    {
        // Check if enough time has passed since the larva feeding
        if (Time.time - lastConsumptionTime > consumptionRate)
        {
            lastConsumptionTime = Time.time;

            bool isPollenAvailable = Hive.instance.pollen > 0;
            bool isNectarAvailable = Hive.instance.nectar > 0;

            // Check if resources are available and the larva is not fully grown
            if (curBuildRoom.roomWorkers.Count > 0 && isPollenAvailable && isNectarAvailable && newBeeConsumption < 100)
            {
                Hive.instance.RemoveMaterial(ResourceType.Nectar);
                Hive.instance.RemoveMaterial(ResourceType.Pollen);
                newBeeConsumption += 1;
                careIdentifire += 1;
            }
            else
            {
                newBeeConsumption -= 1;  // Decrease consumption if resources are not enough
            }
        }
    }

    /// <summary>
    /// Updates the state when the bee is in the growing phase.
    /// </summary>
    void BeeGrowsUpdate()
    {
        if (timeOfStartGrow + timeToGrow > Time.time)
        {
            NewBeeConsumption();

            // Check if the larva did not survive
            if (newBeeConsumption <= 0)
            {
                Log.instance.AddNewLogText(Time.time, "Larva did not survive", Color.red);

                careIdentifire = 0;
                newBeeConsumption = startBeeConsumption;
                SetNurseryState(NurseryState.Empty);
            }
            else if (newBeeConsumption < lowConsumptionLimit)
            {
                Log.instance.AddNewLogText(Time.time, "Larva is starving", Color.yellow);
            }
        }
        else
        {
            Log.instance.AddNewLogText(Time.time, "Larva pupated", Color.black);
            timeOfStartGrow = 0;
            timeOfStartPupation = Time.time;
            SetNurseryState(NurseryState.WaitingForLive);
        }
    }

    /// <summary>
    /// Manages the state when the bee is waiting to emerge as a new adult bee.
    /// </summary>
    void WaitingForLiveUpdate()
    {
        if (timeOfStartPupation + timeToSpawn > Time.time)
        {
            // Waiting for the new bee to emerge
        }
        else
        {
            SetNurseryState(NurseryState.NewBee);
        }
    }

    /// <summary>
    /// Finalizes the development of a new bee, spawning it in the world.
    /// </summary>
    void NewBeeUpdate()
    {
        Log.instance.AddNewLogText(Time.time, "A new bee was born", Color.black);
        Vector3 spawnPosition = new Vector3(0, 0, 0);

        GameObject newBee = Instantiate(beePrefab, spawnPosition, Quaternion.identity);
        SetNewBee(newBee);

        Player.me.units.Add(newBee.GetComponent<Unit>());  // Add the new bee to the player's units

        careIdentifire = 0;
        newBeeConsumption = startBeeConsumption;
        SetNurseryState(NurseryState.Empty);  // Reset the state to Empty after spawning a bee
    }

    /// <summary>
    /// Sets the attributes of the newly spawned bee based on the care it received during development.
    /// </summary>
    /// <param name="newBee">The newly spawned bee GameObject.</param>
    void SetNewBee(GameObject newBee)
    {
        Unit newUnit = newBee.GetComponent<Unit>();

        Player.me.units.Add(newUnit);

        // Calculate the attributes of the new bee based on the care it received
        newUnit.curHp = careIdentifire;
        newUnit.maxHp = 10 + careIdentifire;
        newUnit.minAttackDamage = careIdentifire / 5;
        newUnit.maxAttackDamage = 2 * careIdentifire / 5;
        newUnit.gatherAmount = careIdentifire / 5;
        newUnit.buildAmount = 2 * careIdentifire / 5;

        newUnit.curFeed = newBeeConsumption;
        newUnit.maxFeed = 100;
        newUnit.curEnergy = 50;
        newUnit.maxEnergy = 100;
    }

    /// <summary>
    /// Adds a new egg to the nursery, starting the development process of a new bee.
    /// </summary>
    public void AddNewEgg()
    {
        careIdentifire = 0;
        timeOfStartGrow = Time.time;
        SetNurseryState(NurseryState.BeeGrows);
    }

    /// <summary>
    /// Sets the room state to the specified state and triggers the state change event.
    /// </summary>
    /// <param name="toState">The new state to transition to.</param>
    public void SetRoomState(RoomState toState)
    {
        state = toState;
        if (onStateChange != null)
        {
            onStateChange.Invoke(state);
        }
    }

    /// <summary>
    /// Sets the nursery state to the specified state.
    /// </summary>
    /// <param name="toState">New state.</param>
    public void SetNurseryState(NurseryState toState)
    {
        nurseryState = toState;
        if (onStateChange != null)
        {
            onStateChange.Invoke(state);
        }
    }
}
