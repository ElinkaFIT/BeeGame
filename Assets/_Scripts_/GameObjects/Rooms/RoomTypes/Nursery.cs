//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.Events;

public enum NurseryState
{
    Empty,
    BeeGrows,
    WaitingForLive,
    NewBee
}

public class Nursery : MonoBehaviour
{

    public RoomState state;
    public Room curBuildRoom;
    public NurseryState nurseryState;

    private float timeOfStartGrow;
    public float timeToGrow;

    private float timeOfStartPupation;
    public float timeToSpawn;

    public int newBeeConsumption;
    public float consumptionRate;
    private float lastConsumptionTime;

    public int lowConsumptionLimit;

    private int careIdentifire;
    private int startBeeConsumption;

    public GameObject beePrefab;

    // events
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<RoomState> { }
    public StateChangeEvent onStateChange;

    private void Start()
    {
        startBeeConsumption = newBeeConsumption;
        SetRoomState(RoomState.Blueprint);
    }

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

    void BlueprintUpdate()
    {
        SetRoomState(RoomState.UnderConstruction);
    }

    void UnderConstructionUpdate()
    {
        if (curBuildRoom.concructionDone == true)
        {
            SetRoomState(RoomState.Built);
            gameObject.tag = "Nursery";
        }
    }

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

    void NewBeeConsumption()
    {
        // pokud nastal cas krmeni
        if (Time.time - lastConsumptionTime > consumptionRate)
        {
            lastConsumptionTime = Time.time;


            // tady budou pak jine suroviny
            bool isPollenAvailable = Hive.instance.pollen > 0;
            bool isNectarAvailable = Hive.instance.nectar > 0;

            if (curBuildRoom.roomWorkers.Count > 0 && isPollenAvailable && isNectarAvailable && newBeeConsumption < 100)
            {
                Hive.instance.RemoveMaterial(ResourceType.Nectar);
                Hive.instance.RemoveMaterial(ResourceType.Pollen);
                newBeeConsumption += 1;
                careIdentifire += 1;
            }
            else
            {
                newBeeConsumption -= 1;
            }
        }
    }

    void BeeGrowsUpdate()
    {
        if (timeOfStartGrow + timeToGrow > Time.time)
        {

            NewBeeConsumption();

            // larva zemrela
            if(newBeeConsumption <= 0)
            {
                Log.instance.AddNewLogText(Time.time, "Larva did not survive", Color.red);

                careIdentifire = 0;
                newBeeConsumption = startBeeConsumption;
                SetNurseryState(NurseryState.Empty);
            }
            else if (newBeeConsumption < lowConsumptionLimit)
            {
                // larva hladovi
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
    void WaitingForLiveUpdate()
    {
        if (timeOfStartPupation + timeToSpawn > Time.time)
        {
            // cekani na spawn
        }
        else
        {
            SetNurseryState(NurseryState.NewBee);
        }
    }

    void NewBeeUpdate()
    {
        Log.instance.AddNewLogText(Time.time, "A new bee was born", Color.black);
        Vector3 spawnPosition = new Vector3(0,0,0);

        GameObject newBee = Instantiate(beePrefab, spawnPosition, Quaternion.identity);
        SetNewBee(newBee);

        // add unit to player

        careIdentifire = 0;
        newBeeConsumption = startBeeConsumption;
        SetNurseryState(NurseryState.Empty);
    }

    void SetNewBee(GameObject newBee)
    {
        // vypocet atributu vcely
        // TODO vyrovnat hodnoty (pohrat si s vypoctem)
        Unit newUnit = newBee.GetComponent<Unit>();

        Player.me.units.Add(newUnit);

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

    public void AddNewEgg()
    {
        careIdentifire = 0;
        timeOfStartGrow = Time.time;
        SetNurseryState(NurseryState.BeeGrows);
    }

    public void SetRoomState(RoomState toState)
    {
        state = toState;

        // calling the event
        if (onStateChange != null)
            onStateChange.Invoke(state);
    }

    public void SetNurseryState(NurseryState toState)
    {
        nurseryState = toState;

        // calling the event
        if (onStateChange != null)
            onStateChange.Invoke(state);
    }

}
