using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    public Toggle taskHive;
    public Toggle taskUnit;
    public Toggle taskWater;
    public Toggle taskNectar;

    private bool taskHiveDone;
    private bool taskUnitDone;
    private bool taskWaterDone;
    private bool taskNectarDone;

    private void Start()
    {
        taskHiveDone = false;
        taskUnitDone = false;
    }

    void Update()
    {
        if (!taskHiveDone) { TaskHiveMove(); }
        if (!taskUnitDone) { TaskUnitMove(); }
    }

    private void TaskHiveMove()
    {


        // pokud je kamera na ukolu
        if (true)
        {
            taskHiveDone = true;
            taskHive.isOn = true;
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
