using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    public OpenPanel panel;
    public Toggle taskMap;
    public Toggle taskUnit;

    private bool taskMapDone;
    private bool taskUnitDone;

    private bool wKeyDown;
    private bool aKeyDown;
    private bool sKeyDown;
    private bool dKeyDown;

    private void Start()
    {
        taskMapDone = false;
        taskUnitDone = false;

        wKeyDown = false;
        aKeyDown = false;
        sKeyDown = false;
        dKeyDown = false;

        panel.OpenClosePanel();
    }

    void Update()
    {
        if (!taskMapDone) { TaskMapMove(); }
        if (!taskUnitDone) { TaskUnitMove(); }
    }

    private void TaskMapMove()
    {
        if (Input.GetKeyDown(KeyCode.W)) { wKeyDown = true; }
        if (Input.GetKeyDown(KeyCode.A)) { aKeyDown = true; }
        if (Input.GetKeyDown(KeyCode.S)) { sKeyDown = true; }
        if (Input.GetKeyDown(KeyCode.D)) { dKeyDown = true; }

        // ukol splnen
        if (wKeyDown && aKeyDown && sKeyDown && dKeyDown)
        {
            taskMapDone = true;
            taskMap.isOn = true;
        }
    }

    private void TaskUnitMove()
    {
        foreach(Unit unit in Player.me.units)
        {
            if (unit != null)
            {
                if (unit.state == UnitState.Move)
                {
                    taskUnitDone = true;
                    taskUnit.isOn = true;
                }
            }
        }
        
    }
}
