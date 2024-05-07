//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum QueenState
{
    Normal,
    Hungry,
    QueenIsDying,
    QueenDeath
}

public class Queen : MonoBehaviour
{

    public RoomState state;
    public QueenState queenState;
    public Room curBuildRoom;

    // events
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<RoomState> { }
    public StateChangeEvent onStateChange;

    public float queenConsumption;
    public float consumptionRate;
    private float lastConsumptionTime;

    public int lowConsumptionLimit;
    public int criticalConsumptionLimit;

    public float LayingEggRate;
    private float lastLayingEggTime;

    private void Start()
    {
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
        curBuildRoom.BuildRoom(100);
        if (curBuildRoom.concructionDone == true)
        {
            SetRoomState(RoomState.Built);
            gameObject.tag = "Queen";
        }
    }

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

    void QueenConsumption()
    {
        // pokud nastal cas krmeni
        if (Time.time - lastConsumptionTime > consumptionRate)
        {
            lastConsumptionTime = Time.time;

            bool isPollenAvailable = Hive.instance.pollen > 0;
            bool isNectarAvailable = Hive.instance.nectar > 0;

            // proved krmeni 
            // musi byt aktivni vcela krmicka a dostupne suroviny 
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

    private Room PickingNursery()
    {
        List<Room> rooms = Hive.instance.rooms;
        List<Room> nurseryRooms = new List<Room>();

        // vebere jen prazdne postavene lihne
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

        // vybere vhodne lihne
        foreach (Room nursery in nurseryRooms)
        {
            // pokud je to lihen a je zde pracovnik
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

    void QueenLayingEggs()
    {
        
        // prodlouzi cyklus kladeni dle toho jak je kralovna hladova
        float delay = 50 - queenConsumption;

        // kladeni vajicek
        if (Time.time - lastLayingEggTime > LayingEggRate + delay)
        {
            lastLayingEggTime = Time.time;
            
            Room pickedRoom = PickingNursery();
            if (pickedRoom != null)
            {
                Log.instance.AddNewLogText(Time.time, "Queen lay a new egg", Color.black);
                pickedRoom.gameObject.GetComponent<Nursery>().AddNewEgg();

                // s 60% šancí se do hry pøidá enemy jednotka - zmenit na 0.6
                if(Random.Range(0f, 1f) < 1f) {
                    CommandManager.instance.RunSpawnEnemy();
                }


            }

        }
    }


    // Update funkce
    void NormalQueenUpdate()
    {
        QueenConsumption();

        QueenLayingEggs();

        // pokud dosahne okrajove hladiny konzumace prejdi do jineho stavu
        if (queenConsumption <= lowConsumptionLimit)
        {
            Log.instance.AddNewLogText(Time.time, "Queen is hungry", Color.red);
            SetQueenState(QueenState.Hungry);
        }
    }

    void HungryQueenUpdate()
    {

        QueenConsumption();

        // pokud dosahne okrajove hladiny konzumace prejdi do jineho stavu
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

    void QueenIsDyingUpdate()
    {
        QueenConsumption();

        // blikanim znaci ze se blizi konec hry
        UIComponents.instance.StartGameOverReminder();


        // pokud dosahne okrajove hladiny konzumace prejdi do jineho stavu
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

    void QueenDeathUpdate()
    {
        // konec hry
        GameOver.instance.OpenGameOverMenu();
    }

    public void SetRoomState(RoomState toState)
    {
        state = toState;

        // calling the event
        if (onStateChange != null)
            onStateChange.Invoke(state);
    }

    public void SetQueenState(QueenState toState)
    {
        queenState = toState;

        // calling the event
        if (onStateChange != null)
            onStateChange.Invoke(state);
    }

}
