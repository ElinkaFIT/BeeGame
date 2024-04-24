using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaxFactory : MonoBehaviour
{

    public RoomState state;
    public Room curBuildRoom;

    public float productionRate;
    private float lastProductionTime;

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
            gameObject.tag = "WaxFactory";
        }
    }

    void BuiltUpdate()
    {
        // tvorba nove suroviny
        if (Time.time - lastProductionTime > productionRate)
        {
            lastProductionTime = Time.time;

            if (curBuildRoom.roomWorkers.Count > 0)
            {
                Hive.instance.GainResource(ResourceType.Wax, 1);
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
