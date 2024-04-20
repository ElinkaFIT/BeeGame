using UnityEngine;
using UnityEngine.UI;

public class Level1 : MonoBehaviour
{
    public GameObject nextLevel;

    public Toggle taskHive;
    public Toggle taskUnit;
    public Toggle taskSelect;
    public Toggle taskWater;
    public Toggle taskNectar;

    private bool taskHiveDone;
    private bool taskUnitDone;
    private bool taskSelectDone;
    private bool taskWaterDone;
    private bool taskNectarDone;

    private void Start()
    {
        taskHiveDone = false;
        taskUnitDone = false;
        taskWaterDone = false;
        taskNectarDone = false;
        taskSelectDone = false;

        nextLevel.SetActive(false);

        Invoke(nameof(SetImmortalQueen), 0.1f);
    }

    void SetImmortalQueen()
    {
        GameObject queen = GameObject.FindWithTag("Queen");
        if (queen != null)
        {
            Queen queenScript = queen.GetComponent<Queen>();
            queenScript.queenConsumption = float.PositiveInfinity;

        }
    }

    void Update()
    {
        if (!taskHiveDone) { TaskHiveMove(); }
        if (!taskUnitDone) { TaskUnitMove(); }
        if (!taskWaterDone) { TaskWaterPick(); }
        if (!taskNectarDone) { TaskNectarPick(); }
        if (!taskSelectDone) { TaskSelectUnits(); }

        if (taskHiveDone && taskUnitDone && taskSelectDone && taskNectarDone && taskWaterDone) {
            nextLevel.SetActive(true);
        }
    }

    private void TaskHiveMove()
    {
        Vector3 cameraPosition = Camera.main.transform.position;

        // pokud je kamera na úlu
        if (cameraPosition.x > -10 && (-10 < cameraPosition.y && cameraPosition.y < 30))
        {
            taskHiveDone = true;
            taskHive.isOn = true;
        }
    }

    private void TaskUnitMove()
    {
        foreach (Unit unit in Player.me.units)
        {

            foreach (GameObject room in HiveGenerator.instance.emptyRooms)
            {
                Vector2 roomPos = room.transform.position;
                Vector2 unitPos = unit.transform.position;  

                if (Vector2.Distance(roomPos, unitPos) < 1)
                {
                    taskUnitDone = true;
                    taskUnit.isOn = true;
                }
            }
        }
        
    }

    private void TaskSelectUnits()
    {
        if (UnitSelection.instance.selectedUnits.Count == Player.me.units.Count)
        {
            taskSelectDone = true;
            taskSelect.isOn = true;
        }

    }

    private void TaskWaterPick()
    {
        if (Hive.instance.water >= 20)
        {
            taskWaterDone = true;
            taskWater.isOn = true;
        }

    }

    private void TaskNectarPick()
    {
        if (Hive.instance.nectar >= 20)
        {
            taskNectarDone = true;
            taskNectar.isOn = true;
        }
    }
}
