using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Hive : MonoBehaviour
{
    public int nectar;
    public int nectarCapacity;

    public int water;
    public int waterCapacity;

    public int wax;
    public int waxCapacity;

    public int pollen;
    public int pollenCapacity;

    public int honey;

    public GameObject waxAbsence;

    public List<Room> rooms = new List<Room>();

    public static Hive instance;

    private void Awake()
    {
        instance = this;
    }

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

    public void RemoveMaterial(ResourceType resourceType)
    {
        switch (resourceType)
        {
            case ResourceType.Nectar:
                if (nectar <= 0)
                {
                    Log.instance.AddNewLogText(Time.time, "Amount of nectar is low", Color.black);
                }
                else{
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
            if(NurseryPlacement(curIndicatorPos) == false)
            {
                Debug.Log("Musi sousedit s kralovnou nebo lihni");
                return;
            }
        }

        foreach (Room room in rooms)
        {
            if (curIndicatorPos == room.transform.position)
            {
                Debug.Log("Zde je jiz umisten jiny pokoj");
                return;
            }

        }

        wax -= preset.waxCost;

        GameObject newObject = Instantiate(preset.prefab, curIndicatorPos, Quaternion.identity);
        Room newRoom = newObject.GetComponent<Room>();
        rooms.Add(newRoom);

        GameUI.instance.UpdateWaxText(wax);
    }

    public void OnRemoveBuilding(Room room)
    {
        wax += room.preset.waxCost;
        rooms.Remove(room);
        Destroy(room.gameObject);
        GameUI.instance.UpdateWaxText(wax);
    }

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

    // vrati true pokud sousedi s queen nebo nursery, jinak false
    public bool NurseryPlacement(Vector3 newNursery)
    {
        foreach (Room room in rooms)
        {
            float gridSize = HiveGenerator.instance.grid.cellSize.x;

            if (Vector3.Distance(room.transform.position, newNursery) < 2.2f * HexMath.InnerRadius(gridSize))
            {
                if (room.preset.roomType == RoomType.Queen || room.preset.roomType == RoomType.Nursery)
                    return true;
            }
        }
        return false;
    }

}
