using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsPlacement : MonoBehaviour
{
    private bool currentlyPlacing;
    private bool currentlyBulldozering;

    private RoomPreset curBuildingPreset;

    private float indicatorUpdateRate = 0.05f;
    private float lastUpdateTime;
    private Vector3 curIndicatorPos;

    public GameObject placementIndicator;
    public GameObject bulldozeIndicator;

    private void Awake()
    {
        placementIndicator.SetActive(false);
        bulldozeIndicator.SetActive(false);
    }
    void Update()
    {
        // cancel building placement
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelBuildingPlacement();

        // called every 0.05 seconds
        if (Time.time - lastUpdateTime > indicatorUpdateRate)
        {
            lastUpdateTime = Time.time;
            // get the currently selected tile position
            curIndicatorPos = BuildSelector.instance.GetCurTilePosition();

            if (currentlyPlacing)
                placementIndicator.transform.position = curIndicatorPos;
            else if (currentlyBulldozering)
                bulldozeIndicator.transform.position = curIndicatorPos;
        }
    }

    public void BeginNewBuildingPlacement(RoomPreset preset)
    {
        bulldozeIndicator.SetActive(false);
        currentlyPlacing = true;
        curBuildingPreset = preset;
        placementIndicator.SetActive(true);
    }

    void CancelBuildingPlacement()
    {
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
        bulldozeIndicator.SetActive(false);
    }

    public void ToggleBulldoze()
    {
        placementIndicator.SetActive(false);
        currentlyBulldozering = !currentlyBulldozering;
        bulldozeIndicator.SetActive(currentlyBulldozering);
    }
}
