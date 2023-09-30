using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomsPlacement : MonoBehaviour
{
    private bool currentlyPlacing;

    private RoomPreset curBuildingPreset;

    private float indicatorUpdateRate = 0.05f;
    private float lastUpdateTime;
    private Vector3 curIndicatorPos;

    public GameObject placementIndicator;

    private void Awake()
    {
        placementIndicator.SetActive(false);
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
        }

        if (Input.GetMouseButtonDown(0) && currentlyPlacing)
        {
            PlaceBuilding();
        }
    }

    public void BeginNewBuildingPlacement(RoomPreset preset)
    {
        currentlyPlacing = true;
        curBuildingPreset = preset;
        placementIndicator.SetActive(true);
        // becouse of glitch
        placementIndicator.transform.position = new Vector3(0, 0, -99);
    }

    void CancelBuildingPlacement()
    {
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
    }

    void PlaceBuilding()
    {
        Hive.instance.OnPlaceBuilding(curBuildingPreset, curIndicatorPos);
        CancelBuildingPlacement();
    }

}
