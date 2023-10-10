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


    void Start()
    {
        // GameUI.instance.UpdateUnitCountText(units.Count);
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

    
}
