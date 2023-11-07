using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Hive : MonoBehaviour
{
    public int nectar;
    public int water;
    public int wax;
    public int propolis;

    public GameObject waxAbsence;

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

    public void OnPlaceBuilding(RoomPreset preset, Vector3 curIndicatorPos)
    {
        if (wax < preset.waxCost)
        {
            waxAbsence.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.red;
            waxAbsence.transform.GetChild(1).GetComponent<TextMeshProUGUI>().color = Color.red;
            GameUI.instance.UpdateWaxText(wax);
            return;
        }
        else if (propolis < preset.propolisCost)
        {
            Debug.Log("Neni dostatek propolisu");
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
        propolis -= preset.propolisCost;

        GameObject newObject = Instantiate(preset.prefab, curIndicatorPos, Quaternion.identity);
        Room newRoom = newObject.GetComponent<Room>();
        rooms.Add(newRoom);

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
