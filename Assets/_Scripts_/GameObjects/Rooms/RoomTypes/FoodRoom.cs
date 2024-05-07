//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.Events;

public class FoodRoom : MonoBehaviour
{

    public RoomState state;
    public Room curBuildRoom;

    public float feedRate;
    private float lastFeedTime;

    public int roomCapacity;
    public int eatAmount;

    // events
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<RoomState> { }
    public StateChangeEvent onStateChange;

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
            gameObject.tag = "FoodRoom";
        }
    }

    void BuiltUpdate()
    {
        // tvorba nove suroviny
        if (Time.time - lastFeedTime > feedRate)
        {
            lastFeedTime = Time.time;

            bool isNectarAvailable = Hive.instance.nectar > 0;

            if (curBuildRoom.roomWorkers.Count > 0 && isNectarAvailable)
            {
                Hive.instance.RemoveMaterial(ResourceType.Nectar);

                foreach (Unit worker in curBuildRoom.roomWorkers)
                {

                    if(worker.curFeed < worker.maxFeed) {
                        worker.curFeed += eatAmount;
                    }
                }

            }


        }
    }

    public void SetRoomState(RoomState toState)
    {
        state = toState;

        // calling the event
        if (onStateChange != null)
            onStateChange.Invoke(state);
    }

}
