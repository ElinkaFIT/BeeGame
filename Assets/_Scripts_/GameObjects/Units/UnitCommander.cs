using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCommander : MonoBehaviour
{
    public LayerMask layerMask;
    private Vector2 target;

    // components
    private UnitSelection unitSelection;
    private Camera cam;
    void Awake()
    {
        // get the components
        unitSelection = GetComponent<UnitSelection>();
        cam = Camera.main;
    }

    void Update()
    {

        if (Input.GetMouseButtonDown(1) && unitSelection.AreUnitsSelected())
        {
            Unit[] selectedUnits = unitSelection.GetSelectedUnits();
            target = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 origin = new Vector2(cam.ScreenToWorldPoint(Input.mousePosition).x,
                                         cam.ScreenToWorldPoint(Input.mousePosition).y);
            RaycastHit2D hit = Physics2D.Raycast(origin, -Vector2.up, 0f);

            if (hit)
            {
                unitSelection.RemoveNullUnitsFromSelection();

                if (hit.collider.CompareTag("Resource"))
                {
                    UnitsGatherResource(hit.collider.GetComponent<ResourceSource>(), selectedUnits);

                }
                else if (hit.collider.CompareTag("FogTile"))
                {
                    UnitsSearching(hit.collider.GetComponent<ResourceTile>(), selectedUnits);

                }
                else if (hit.collider.CompareTag("Room"))
                {
                    UnitsBuildRoom(hit.collider.GetComponent<Room>(), selectedUnits);

                }
                else if (hit.collider.CompareTag("Queen") || hit.collider.CompareTag("Nursery") 
                    || hit.collider.CompareTag("RestRoom") || hit.collider.CompareTag("FoodRoom") 
                    || hit.collider.CompareTag("HoneyFactory") || hit.collider.CompareTag("WaxFactory"))
                {
                    UnitsToWork(hit.collider.GetComponent<Room>(), selectedUnits);

                }
                else if (hit.collider.CompareTag("AIUnit"))
                {
                    UnitAI enemy = hit.collider.gameObject.GetComponent<UnitAI>();
                    UnitsAttackEnemy(enemy, selectedUnits);

                }
                else if (hit.collider.CompareTag("Ground"))
                {
                    UnitsMoveToPosition(target, selectedUnits);
                }

            }

        }
    }

    void UnitsAttackEnemy(UnitAI target, Unit[] units)
    {
        for (int x = 0; x < units.Length; x++)
            units[x].AttackUnit(target);
    }


    void UnitsMoveToPosition(Vector2 target, Unit[] units)
    {
        // vypocitej presnou polohu jednotky
        Vector2[] destination = GetUnitPosition(target, units.Length, 1);
        // pohyb kazde jednotky
        for (int x = 0; x < units.Length; x++)
        {
            units[x].MoveToPosition(destination[x]);
        }
    }

    void UnitsGatherResource(ResourceSource resource, Unit[] units)
    {
        Vector2[] destinations = GetUnitPosition(resource.transform.position, units.Length, 1);
        for (int x = 0; x < units.Length; x++)
        {
            units[x].GatherResource(resource, destinations[x]);
        }

    }

    public static Vector2[] GetUnitPosition(Vector2 moveToPos, int numUnits, float unitGap)
    {
        Vector2[] destinations = new Vector2[numUnits];

        int rows = Mathf.RoundToInt(Mathf.Sqrt(numUnits));
        int cols = Mathf.CeilToInt((float)numUnits / (float)rows);

        int curRow = 0;
        int curCol = 0;
        float width = ((float)rows - 1) * unitGap;
        float length = ((float)cols - 1) * unitGap;
        for (int x = 0; x < numUnits; x++)
        {
            destinations[x] = moveToPos + (new Vector2(curRow, curCol) * unitGap) - new Vector2(length / 2, width / 2);
            curCol++;
            if (curCol == rows)
            {
                curCol = 0;
                curRow++;
            }
        }
        return destinations;
    }

    void UnitsSearching(ResourceTile tile, Unit[] units)
    {
        if (units.Length == 1)
        {
            units[0].Searching(tile, tile.transform.position);
        }
        else
        {
            for (int x = 0; x < units.Length; x++)
            {
                units[x].Searching(tile, tile.transform.position);
            }

        }
    }

    void UnitsBuildRoom(Room room, Unit[] units)
    {
        if (units.Length == 1)
        {
            units[0].BuildRoom(room, room.transform.position);
        }
        else
        {
            units[0].BuildRoom(room, room.transform.position);
        }
    }

    void UnitsToWork(Room room, Unit[] units)
    {
        if (units.Length == 1)
        {
            units[0].WorkInRoom(room, room.transform.position);
        }
        else
        {
            units[0].WorkInRoom(room, room.transform.position);
        }
    }
}
