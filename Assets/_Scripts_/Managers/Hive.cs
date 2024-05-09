//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Manages the resources and building placements within the hive.
/// </summary>
public class Hive : MonoBehaviour
{
    // Current and current maximum capacity of nectar in the hive.
    public int nectar;
    public int nectarCapacity;

    // Current and current maximum capacity of water in the hive.
    public int water;
    public int waterCapacity;

    // Current and current maximum capacity of wax in the hive.
    public int wax;
    public int waxCapacity;

    // Current and current maximum capacity of pollen in the hive.
    public int pollen;
    public int pollenCapacity;

    // Current amount of honey in the hive.
    public int honey;

    // Notification object when there's not enough wax.
    public GameObject waxAbsence;

    // List of all rooms in the hive.
    public List<Room> rooms = new List<Room>();

    // Singleton instance of the Hive.
    public static Hive instance;

    /// <summary>
    /// Ensures the Hive instance is a singleton.
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Initializes the Hive by loading game data and updating the UI.
    /// </summary>
    private void Start()
    {
        //SaveManager.instance.LoadGame();

        GameUI.instance.UpdateNectarCapacity(nectarCapacity);
        GameUI.instance.UpdateNectarText(nectar);

        GameUI.instance.UpdateWaterCapacity(waterCapacity);
        GameUI.instance.UpdateWaterText(water);

        GameUI.instance.UpdateWaxCapacity(waxCapacity);
        GameUI.instance.UpdateWaxText(wax);

        GameUI.instance.UpdatePollenCapacity(pollenCapacity);
        GameUI.instance.UpdatePollenText(pollen);

        GameUI.instance.UpdateHoneyText(honey);
    }

    /// <summary>
    /// Decreases the specified resource by one if available and updates the UI.
    /// </summary>
    /// <param name="resourceType">Type of the resource to remove.</param>
    public void RemoveMaterial(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Nectar:
                if (nectar <= 0)
                {
                    Log.instance.AddNewLogText(Time.time, "Amount of nectar is low", Color.black);
                }
                else
                {
                    nectar--;
                    GameUI.instance.UpdateNectarText(nectar);
                }
                break;
            case ResourceType.Water:
                if (water <= 0)
                {
                    Log.instance.AddNewLogText(Time.time, "Amount of water is low", Color.black);
                }
                else
                {
                    water--;
                    GameUI.instance.UpdateWaterText(water);
                }
                break;
            case ResourceType.Wax:
                if (wax <= 0)
                {
                    Log.instance.AddNewLogText(Time.time, "Amount of wax is low", Color.black);
                }
                else
                {
                    wax--;
                    GameUI.instance.UpdateWaxText(wax);
                }
                break;
            case ResourceType.Pollen:
                if (pollen <= 0)
                {
                    Log.instance.AddNewLogText(Time.time, "Amount of pollen is low", Color.black);
                }
                else
                {
                    pollen--;
                    GameUI.instance.UpdatePollenText(pollen);
                }
                break;
            case ResourceType.Honey:
                if (honey <= 0)
                {
                    Log.instance.AddNewLogText(Time.time, "Amount of honey is low", Color.black);
                }
                else
                {
                    honey--;
                    GameUI.instance.UpdateHoneyText(honey);
                }
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Places a building in the hive if enough resources are available.
    /// </summary>
    /// <param name="preset">Preset of the room to build.</param>
    /// <param name="curIndicatorPos">Position to place the building.</param>
    public void OnPlaceBuilding(RoomPreset preset, Vector3 curIndicatorPos)
    {
        if (wax < preset.waxCost)
        {
            waxAbsence.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
            waxAbsence.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.red;
            GameUI.instance.UpdateWaxText(wax);
            return;
        }

        if (preset.roomType == RoomType.Nursery)
        {
            if (NurseryPlacement(curIndicatorPos) == false)
            {
                Log.instance.AddNewLogText(Time.time, "Must border queen or hatchery", Color.grey);
                return;
            }
        }

        foreach (Room room in rooms)
        {
            if (curIndicatorPos == room.transform.position)
            {
                Log.instance.AddNewLogText(Time.time, "Object already built here", Color.grey);
                return;
            }
        }

        wax -= preset.waxCost;

        GameObject newObject = Instantiate(preset.prefab, curIndicatorPos, Quaternion.identity);
        Room newRoom = newObject.GetComponent<Room>();
        rooms.Add(newRoom);

        GameUI.instance.UpdateWaxText(wax);
    }

    /// <summary>
    /// Removes a building from the hive and refunds the wax cost.
    /// </summary>
    /// <param name="room">The room to be removed.</param>
    public void OnRemoveBuilding(Room room)
    {
        wax += room.preset.waxCost;
        rooms.Remove(room);
        Destroy(room.gameObject);
        GameUI.instance.UpdateWaxText(wax);
    }

    /// <summary>
    /// Adds a specified amount of a resource to the hive, respecting capacity limits.
    /// </summary>
    /// <param name="resourceType">Type of the resource to add.</param>
    /// <param name="amount">Amount of the resource to add.</param>
    public void GainResource(ResourceType resourceType, int amount)
    {
        switch (resourceType)
        {
            case ResourceType.Nectar:
                {
                    int newNectarValue = nectar + amount;
                    if (newNectarValue > nectarCapacity)
                    {
                        nectar = nectarCapacity;
                        Log.instance.AddNewLogText(Time.time, "Capacity of nectar is full", Color.red);
                    }
                    else { nectar = newNectarValue; }
                    GameUI.instance.UpdateNectarText(nectar);
                    break;
                }
            case ResourceType.Water:
                {
                    int newWaterValue = water + amount;
                    if (newWaterValue > waterCapacity)
                    {
                        water = waterCapacity;
                        Log.instance.AddNewLogText(Time.time, "Capacity of water is full", Color.red);
                    }
                    else { water = newWaterValue; }
                    GameUI.instance.UpdateWaterText(water);
                    break;
                }
            case ResourceType.Pollen:
                {
                    int newPollenValue = pollen + amount;
                    if (newPollenValue > pollenCapacity)
                    {
                        pollen = pollenCapacity;
                        Log.instance.AddNewLogText(Time.time, "Capacity of pollen is full", Color.red);
                    }
                    else { pollen = newPollenValue; }
                    GameUI.instance.UpdatePollenText(pollen);
                    break;
                }
            case ResourceType.Wax:
                {
                    int newValue = wax + amount;
                    if (newValue > waxCapacity)
                    {
                        wax = waxCapacity;
                        Log.instance.AddNewLogText(Time.time, "Capacity of wax is full", Color.red);
                    }
                    else { wax = newValue; }
                    GameUI.instance.UpdateWaxText(wax);
                    break;
                }
            case ResourceType.Honey:
                {
                    honey += amount;
                    GameUI.instance.UpdateHoneyText(honey);
                    break;
                }
        }
    }

    /// <summary>
    /// Checks if a nursery can be placed next to a queen or another nursery.
    /// </summary>
    /// <param name="newNursery">Position to check for nursery placement.</param>
    /// <returns>True if placement is valid, otherwise false.</returns>
    public bool NurseryPlacement(Vector3 newNursery)
    {
        foreach (Room room in rooms)
        {
            float gridSize = HiveGenerator.instance.grid.cellSize.x;

            if ( Vector3.Distance(room.transform.position, newNursery) < 2.2f * HexMath.InnerRadius(gridSize))
            {
                if (room.preset.roomType == RoomType.Queen || room.preset.roomType == RoomType.Nursery)
                    return true;
            }
        }
        return false;
    }
}
