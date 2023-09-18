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
        wax -= room.preset.waxCost;
        propolis -= room.preset.propolisCost;
        //room.Add(room);
        GameUI.instance.UpdateWaxText(wax);
    }

    public void OnRemoveBuilding(Room room)
    {
        wax += room.preset.waxCost;
        propolis += room.preset.propolisCost;
        //buildings.Remove(building);
        Destroy(room.gameObject);
        GameUI.instance.UpdateWaterText(water);
    }

    public void GainResource(ResourceType resourceType, int amount)
    {
        switch (resourceType)
        {
            case ResourceType.Food:
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
