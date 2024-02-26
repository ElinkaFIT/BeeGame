using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public enum ResourceTileState
{
    Fog,
    Discovering,
    Exposed
}

public class ResourceTile : MonoBehaviour
{

    public ResourceTileState state;
    public BuildProgressBar progressBar;

    public int buildProgress;
    public int buildModifier;

    // Start is called before the first frame update
    void Start()
    {
        SetRoomState(ResourceTileState.Fog);
    }

    void Update()
    {
        switch (state)
        {
            case ResourceTileState.Fog:
                {
                    FogUpdate();
                    break;
                }
            case ResourceTileState.Discovering:
                {
                    DiscoveringUpdate();
                    break;
                }
            case ResourceTileState.Exposed:
                {
                    ExposedUpdate();
                    break;
                }

        }
    }

    private void ExposedUpdate()
    {
        
    }

    private void DiscoveringUpdate()
    {
        
    }

    private void FogUpdate()
    {
        
    }

    public void SearchArea(int amount)
    {
        if (buildProgress >= 100)
        {
            progressBar.CloseProgressBar();
            return;
        }

        buildProgress = buildProgress + buildModifier + amount;
        progressBar.UpdateProgressBar(buildProgress, 100);

    }

    public void SetRoomState(ResourceTileState toState)
    {
        state = toState;
    }
}
