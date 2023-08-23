using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour
{
    public bool isMe;
    public static Player me;

    [Header("Units")]
    public List<Unit> units = new List<Unit>();

    [Header("Resources")]
    public int food;
    public int water;

    void Start()
    {
        GameUI.instance.UpdateUnitCountText(units.Count);
        GameUI.instance.UpdateFoodText(food);
        GameUI.instance.UpdateWaterText(water);
    }

    void Awake()
    {
        if (isMe)
            me = this;
    }


    public bool IsMyUnit(Unit unit)
    {
        return units.Contains(unit);
    }

    public void CreateNewUnit()
    {
        
    }

    public void GainResource(ResourceType resourceType, int amount)
    {
        switch (resourceType)
        {
            case ResourceType.Food:
            {
                food += amount;
                GameUI.instance.UpdateFoodText(food);
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
