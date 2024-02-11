using System.Collections;
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

    public int queenConsumption;
    public float consumptionRate;
    private float lastConsumptionTime;

    public int lowConsumptionLimit;
    public int criticalConsumptionLimit;

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

            bool isWaterAvailable = Hive.instance.water > 0;
            bool isNectarAvailable = Hive.instance.nectar > 0;

            // proved krmeni 
            // musi byt aktivni vcela krmicka a dostupne suroviny 
            if (curBuildRoom.roomWorkers.Count > 0 && isWaterAvailable && isNectarAvailable && queenConsumption < 100)
            {
                Hive.instance.RemoveMaterial(ResourceType.Nectar);
                Hive.instance.RemoveMaterial(ResourceType.Water);
                queenConsumption += 1;
            }
            else
            {
                queenConsumption -= 1;
            }
        }


    }

    void NormalQueenUpdate()
    {
        QueenConsumption();

        // pokud dosahne okrajove hladiny konzumace prejdi do jineho stavu
        if (queenConsumption <= lowConsumptionLimit)
        {
            SetQueenState(QueenState.Hungry);
        }
    }

    void HungryQueenUpdate()
    {

        QueenConsumption();

        // pokud dosahne okrajove hladiny konzumace prejdi do jineho stavu
        if (queenConsumption <= criticalConsumptionLimit)
        {
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
        GameOver.instance.OpenGameOverMenu();
        // konec hry
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
