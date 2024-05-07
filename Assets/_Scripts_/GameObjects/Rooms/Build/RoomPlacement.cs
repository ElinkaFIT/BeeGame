//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
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
    public Color colorCorrectPosition;
    public Color colorBase;

    private void Awake()
    {
        placementIndicator.SetActive(false);
    }
    void Update()
    {
        // cancel building placement
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelBuildingPlacement();
        }

        // called every 0.05 seconds
        if (Time.time - lastUpdateTime > indicatorUpdateRate)
        {
            lastUpdateTime = Time.time;
            // get the currently selected tile position
            
            curIndicatorPos = BuildSelector.instance.GetCurTilePosition();

            if (currentlyPlacing)
            {
                placementIndicator.transform.position = curIndicatorPos;
            }
        }

        if (Input.GetMouseButtonDown(0) && currentlyPlacing)
        {
            PlaceBuilding();
        }
    }

    public void BeginNewBuildingPlacement(RoomPreset preset)
    {
        // clear indicator where i can build
        ClearCorrectPositions();

        currentlyPlacing = true;
        curBuildingPreset = preset;
        placementIndicator.SetActive(true);
        // becouse of glitch
        placementIndicator.transform.position = new Vector3(0, 0, -99);

        // Show where i can build
        ShowCorrectPositions();
    }

    void CancelBuildingPlacement()
    {
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
        // clear indicator where i can build
        ClearCorrectPositions();
    }

    void PlaceBuilding()
    {
        Hive.instance.OnPlaceBuilding(curBuildingPreset, curIndicatorPos);
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            CancelBuildingPlacement();
        }
    }

    // correct position indicator
    void ShowCorrectPositions()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("EmptyRoom");
        foreach (GameObject gameObject in gameObjects)
        {
            if (IsItCorrectPlacement(gameObject))
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = colorCorrectPosition;
            }
        }
    }
    bool IsItCorrectPlacement(GameObject emptyRoom)
    {
        // získání existujícíchobjektù
        List<GameObject> existingRooms = new List<GameObject>();

        existingRooms.AddRange(GameObject.FindGameObjectsWithTag("Room"));
        existingRooms.AddRange(GameObject.FindGameObjectsWithTag("Queen"));
        existingRooms.AddRange(GameObject.FindGameObjectsWithTag("Nursery"));
        existingRooms.AddRange(GameObject.FindGameObjectsWithTag("RestRoom"));
        existingRooms.AddRange(GameObject.FindGameObjectsWithTag("FoodRoom"));
        existingRooms.AddRange(GameObject.FindGameObjectsWithTag("HoneyFactory"));
        existingRooms.AddRange(GameObject.FindGameObjectsWithTag("WaxFactory"));


        Vector2 emptyRoomPos = new Vector2(emptyRoom.transform.position.x, emptyRoom.transform.position.y);
        bool nearNursery = false;

        foreach (GameObject room in existingRooms)
        {
            Vector2 roomPos = new Vector2(room.transform.position.x, room.transform.position.y);
            RoomType roomType = room.GetComponent<Room>().preset.roomType;

            //nursery must be near *queen * or nursery
            if (curBuildingPreset.roomType == RoomType.Nursery && roomType == RoomType.Queen)
            {
                Grid grid = HiveGenerator.instance.grid;
                if (Vector2.Distance(roomPos, emptyRoomPos) < 2)
                {
                    nearNursery = true;
                }
            }

            // Nursery must be near Queen or *Nursery*
            if (curBuildingPreset.roomType == RoomType.Nursery && roomType == RoomType.Nursery)
            {

                Grid grid = HiveGenerator.instance.grid;
                if (Vector2.Distance(roomPos, emptyRoomPos) < 2)
                {
                    nearNursery = true;
                }
            }

            // Everithing is good except where are already rooms
            if (roomPos == emptyRoomPos)
            {
                return false;
            }
        }

        if (!nearNursery && curBuildingPreset.roomType == RoomType.Nursery)
        {
            return false;
        }

        return true;
    }
    void ClearCorrectPositions()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("EmptyRoom");
        foreach (GameObject gameObject in gameObjects)
        {

            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = colorBase;

        }
    }

}
