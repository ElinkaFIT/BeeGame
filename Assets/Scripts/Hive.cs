using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hive : MonoBehaviour
{
    public int nectar;
    public int water;
    public int wax;
    public int propolis;

    public List<Room> rooms = new List<Room>();

    public static Hive instance;

    private void Awake()
    {
        instance = this;
        GameUI.instance.UpdateWaterText(water);
        GameUI.instance.UpdateNectarText(nectar);
        GameUI.instance.UpdateWaxText(wax);
        GameUI.instance.UpdatePropolisText(propolis);
    }

    public void OnPlaceBuilding(Room room)
    {
        if (wax < room.preset.waxCost)
        {
            Debug.Log("Neni dostatek vosku");
            return;
        }
        else if (propolis < room.preset.propolisCost)
        {
            Debug.Log("Neni dostatek propolisu");
            return;
        }
        wax -= room.preset.waxCost;
        propolis -= room.preset.propolisCost;
        rooms.Add(room);
        GameUI.instance.UpdateWaxText(wax);
    }

    public void OnRemoveBuilding(Room room)
    {
        wax += room.preset.waxCost;
        propolis += room.preset.propolisCost;
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
                    nectar += amount;
                    GameUI.instance.UpdateNectarText(nectar);
                    break;
                }
            case ResourceType.Water:
                {
                    water += amount;
                    GameUI.instance.UpdateWaterText(water);
                    break;
                }
        }
    }
}
