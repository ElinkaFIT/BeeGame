//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.Events;

public class Warehouse : MonoBehaviour
{

    public RoomState state;
    public Room curBuildRoom;
    public int addingCapacity;

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
            // zvisi maximalni pocet surovin 
            IncreaseCapacity();

            SetRoomState(RoomState.Built);
        }
    }

    private void IncreaseCapacity()
    {
        int newNectarCapacity = Hive.instance.nectarCapacity + addingCapacity;
        Hive.instance.nectarCapacity = newNectarCapacity;
        GameUI.instance.UpdateNectarCapacity(newNectarCapacity);

        int newWaterCapacity = Hive.instance.waterCapacity + addingCapacity;
        Hive.instance.waterCapacity = newWaterCapacity;
        GameUI.instance.UpdateWaterCapacity(newWaterCapacity);
    }

    public void SetRoomState(RoomState toState)
    {
        state = toState;

        // calling the event
        if (onStateChange != null)
            onStateChange.Invoke(state);
    }

}
