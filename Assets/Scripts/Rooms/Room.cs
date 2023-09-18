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

    // events
    [System.Serializable]
    public class StateChangeEvent : UnityEvent<RoomState> { }
    public StateChangeEvent onStateChange;

    private void Start()
    {
        SetState(RoomState.Blueprint);
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
        // na zacatku odeberu suroviny - je to potreba mozna uz driv

        // pokud je tu vcela pridelena zacne se to stavet, tedy prejde do dalsiho stavu
        // jinak se nic nedeje ale muzu zrusit bluprint, takze cely objekt
    }

    void UnderConstructionUpdate()
    {
        // naplnuje se progressBar, pokud je na 100% tak prechazime do Built statu 
        // musi tu byt pridelena vcela
        // jeste muzu zrusit stavbu - pokud zrusim vrati se mi suroviny

        //nakonec se odectou naklady za postaveni
    }

    void BuiltUpdate() 
    {
        // tady se deji pak velke veci
    }

    void SetState(RoomState toState)
    {
        state = toState;
        // calling the event
        if (onStateChange != null)
            onStateChange.Invoke(state);
    }

}
