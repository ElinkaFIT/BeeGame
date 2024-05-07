//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.Events;

public class RestRoom : MonoBehaviour
{

    public RoomState state;
    public Room curBuildRoom;

    public int restCapacity;
    public int restRate;

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
            gameObject.tag = "RestRoom";
        }
    }

    void BuiltUpdate()
    {
        if (curBuildRoom.roomWorkers.Count > 0) {
            Sleep();
        }
    }

    void Sleep()
    {
        foreach(var worker in curBuildRoom.roomWorkers)
        {
            worker.curEnergy = 100;
        }
    }
    public bool IsRestRoomAvailable()
    {
        return restCapacity > 0 ? true : false;
    }

    public void SetRoomState(RoomState toState)
    {
        state = toState;

        // calling the event
        if (onStateChange != null)
            onStateChange.Invoke(state);
    }

}
