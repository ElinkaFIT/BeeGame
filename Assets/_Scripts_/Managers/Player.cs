using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class Player : MonoBehaviour
{
    public bool isMe;
    public static Player me;
    public GameOver gameOver;
    public TextMeshPro beeCount;

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


    private void Update()
    {
        // bee text
        GameUI.instance.UpdateBeesText(me.units.Count);

        // pokud neni jiz zadna jednotka nastane konec hry
        if (me.units.Count == 0)
        {
            gameOver.OpenGameOverMenu();
        }
    }

    public bool IsMyUnit(Unit unit)
    {
        return units.Contains(unit);
        
    }

    public void CreateNewUnit()
    {
        
    }

    
}
