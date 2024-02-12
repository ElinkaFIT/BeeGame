using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Presets;
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
                    EmptyUpdate();
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

    void EmptyUpdate()
    {
        
    }

    void NewBeeConsumption()
    {
        // pokud nastal cas krmeni
        if (Time.time - lastConsumptionTime > consumptionRate)
        {
            lastConsumptionTime = Time.time;


            // tady budou pak jine suroviny
            bool isWaterAvailable = Hive.instance.water > 0;
            bool isNectarAvailable = Hive.instance.nectar > 0;

            if (curBuildRoom.roomWorkers.Count > 0 && isWaterAvailable && isNectarAvailable && newBeeConsumption < 100)
            {
                Hive.instance.RemoveMaterial(ResourceType.Nectar);
                Hive.instance.RemoveMaterial(ResourceType.Water);
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
                Debug.Log("UPOZORNENI");
                Debug.Log("larva zemrela");

                careIdentifire = 0;
                newBeeConsumption = startBeeConsumption;
                SetNurseryState(NurseryState.Empty);
            }
            else if (newBeeConsumption < lowConsumptionLimit)
            {
                Debug.Log("UPOZORNENI");
                Debug.Log("larva hladovi");
            }
        }
        else
        {
            Debug.Log("cas dobehl");
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
        Debug.Log(careIdentifire);
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
        Unit newUnit = newBee.GetComponent<Unit>();

        Player.me.units.Add(newUnit);

        newUnit.curHp = careIdentifire;
        newUnit.maxHp = 10 + careIdentifire;
        newUnit.minAttackDamage = careIdentifire / 5;
        newUnit.maxAttackDamage = 2 * careIdentifire / 5;
        newUnit.gatherAmount = careIdentifire / 5;
        newUnit.buildAmount = 2 * careIdentifire / 5;

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
