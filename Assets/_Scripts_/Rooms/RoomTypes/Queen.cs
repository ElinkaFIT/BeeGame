using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Queen : MonoBehaviour
{

    public RoomState state;
    public Room curBuildRoom;

    // events
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<RoomState> { }
    public StateChangeEvent onStateChange;

    private int queenHunger;

    private void Start()
    {
        queenHunger = 0;   
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
        }
    }

    void BuiltUpdate()
    {
        // pokud je tam vcela tak blizke postavene lihne prejdou do stavu vajicko
        // a odebira se jim materi kasicka a voda
        // zaroven pokud dojde vcela tak se postupne zmensuje queensHunger

        // pokud zde neni vcela tak se pocita cas queensHunger, pokud queensHunger se rovna nìjaký èas
        // tak se zobrazí oznámení že nìco není v poøádku, pokud queensHunger dosáhne delšího, je game over

    }

    public void SetRoomState(RoomState toState)
    {
        state = toState;

        // calling the event
        if (onStateChange != null)
            onStateChange.Invoke(state);
    }

}
