using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum RoomState
{
    Blueprint,
    UnderConstruction,
    Built
}

public class Room : MonoBehaviour
{
    public RoomPreset preset;
    public RoomState state;
    public BuildProgressBar progressBar;
    public GameObject doneIcon;


    public int buildProgress;

    // events
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<RoomState> { }
    public StateChangeEvent onStateChange;

    private void Start()
    {
        doneIcon.SetActive(false);
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
        
    }

    void UnderConstructionUpdate()
    {
        
    }

    void BuiltUpdate() 
    {
        
    }

    public void BuildRoom(int amount)
    {
        if (buildProgress >= 100)
        {
            SetRoomState(RoomState.Built);
            progressBar.CloseProgressBar();
            doneIcon.SetActive(true);
            return;
        }

        buildProgress += amount;
        progressBar.UpdateProgressBar(buildProgress, 100);

    }

    public void SetRoomState(RoomState toState)
    {
        state = toState;
        // calling the event
        if (onStateChange != null)
            onStateChange.Invoke(state);
    }

}
