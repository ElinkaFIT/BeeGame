//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the placement of rooms within the game, including interaction with the player for building placement.
/// </summary>
public class RoomsPlacement : MonoBehaviour
{
    private bool currentlyPlacing;              // Flag indicating if a room is currently being placed

    private RoomPreset curBuildingPreset;       // Current building preset being placed

    private float indicatorUpdateRate = 0.05f;  // Rate at which the placement indicator updates its position
    private float lastUpdateTime;               // Time when the placement indicator was last updated
    private Vector3 curIndicatorPos;            // Current position of the placement indicator

    public GameObject placementIndicator;       // The visual indicator for room placement
    public Color colorCorrectPosition;          // Color to show when the placement position is correct
    public Color colorBase;                     // Default color of the placement indicator

    /// <summary>
    /// Initializes the component by hiding the placement indicator.
    /// </summary>
    private void Awake()
    {
        placementIndicator.SetActive(false);
    }

    /// <summary>
    /// Handles user input and updates the placement indicator.
    /// </summary>
    void Update()
    {
        // Cancel building placement
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CancelBuildingPlacement();
        }

        // Update the placement indicator every 0.05 seconds
        if (Time.time - lastUpdateTime > indicatorUpdateRate)
        {
            lastUpdateTime = Time.time;
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

    /// <summary>
    /// Starts the placement process for a new building based on the given preset.
    /// </summary>
    /// <param name="preset">The building preset to place.</param>
    public void BeginNewBuildingPlacement(RoomPreset preset)
    {
        ClearCorrectPositions();

        currentlyPlacing = true;
        curBuildingPreset = preset;
        placementIndicator.SetActive(true);

        // Position the indicator to avoid visual glitches
        placementIndicator.transform.position = new Vector3(0, 0, -99); 

        ShowCorrectPositions(); // Update the display to show where the player can build
    }

    /// <summary>
    /// Cancels the current building placement process.
    /// </summary>
    void CancelBuildingPlacement()
    {
        currentlyPlacing = false;
        placementIndicator.SetActive(false);
        ClearCorrectPositions(); // Clear any position highlights
    }

    /// <summary>
    /// Finalizes the building placement at the current indicator position.
    /// </summary>
    void PlaceBuilding()
    {
        Hive.instance.OnPlaceBuilding(curBuildingPreset, curIndicatorPos);
        if (!Input.GetKey(KeyCode.LeftShift))
        {
            CancelBuildingPlacement();
        }
    }

    /// <summary>
    /// Highlights positions where the player can place buildings according to game rules.
    /// </summary>
    void ShowCorrectPositions()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("EmptyRoom");
        foreach (GameObject gameObject in gameObjects)
        {
            if (IsItCorrectPlacement(gameObject))
            {
                SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
                spriteRenderer.color = colorCorrectPosition; // Highlight the position
            }
        }
    }

    /// <summary>
    /// Determines if placing a building at the specified empty room position is allowed.
    /// </summary>
    /// <param name="emptyRoom">The empty room to check.</param>
    /// <returns>True if the building can be placed; otherwise, false.</returns>
    bool IsItCorrectPlacement(GameObject emptyRoom)
    {
        // Get existing object
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

            // Nursery must be placed near Queen or Nursery
            if (curBuildingPreset.roomType == RoomType.Nursery && roomType == RoomType.Queen)
            {
                Grid grid = HiveGenerator.instance.grid;
                if (Vector2.Distance(roomPos, emptyRoomPos) < 2)
                {
                    nearNursery = true;
                }
            }
            if (curBuildingPreset.roomType == RoomType.Nursery && roomType == RoomType.Nursery)
            {

                Grid grid = HiveGenerator.instance.grid;
                if (Vector2.Distance(roomPos, emptyRoomPos) < 2)
                {
                    nearNursery = true;
                }
            }

            // Prevent placement on top of existing rooms
            if (roomPos == emptyRoomPos)
            {
                return false;
            }
        }

        // Ensure nurseries are placed correctly, or return true for other types
        if (!nearNursery && curBuildingPreset.roomType == RoomType.Nursery)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// Resets the color of all potential building positions back to the base color.
    /// </summary>
    void ClearCorrectPositions()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("EmptyRoom");
        foreach (GameObject gameObject in gameObjects)
        {
            SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
            spriteRenderer.color = colorBase; // Reset the color
        }
    }
}
