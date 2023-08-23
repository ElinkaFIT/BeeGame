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

                if (hit.collider.CompareTag("Ground"))
                {
                    UnitsMoveToPosition(target, selectedUnits);
                }
                else if (hit.collider.CompareTag("Resource"))
                {
                    UnitsGatherResource(hit.collider.GetComponent<ResourceSource>(), selectedUnits);

                }
                else if (hit.collider.CompareTag("Unit"))
                {
                    Unit enemy = hit.collider.gameObject.GetComponent<Unit>();
                    if (!Player.me.IsMyUnit(enemy))
                    {
                        UnitsAttackEnemy(enemy, selectedUnits);
                    }
                }

            }

        }
    }

    void UnitsAttackEnemy(Unit target, Unit[] units)
    {
        for (int x = 0; x < units.Length; x++)
            units[x].AttackUnit(target);
    }


    void UnitsMoveToPosition(Vector2 target, Unit[] units)
    {
        // vypocitej presnou polohu jednotky
        Vector2[] destination = UnitMovement.GetUnitGroupDestinations(target, units.Length, 1);
        // pohyb kazde jednotky
        for (int x = 0; x < units.Length; x++)
        {
            units[x].MoveToPosition(destination[x]);
        }
    }

    void UnitsGatherResource(ResourceSource resource, Unit[] units)
    {
        if (units.Length == 1)
        {
            units[0].GatherResource(resource, UnitMovement.GetUnitDestinationAroundResource(resource.transform.position));
        }
        else
        {
            Vector3[] destinations = UnitMovement.GetUnitGroupDestinationsAroundResource(resource.transform.position, units.Length);
            for (int x = 0; x < units.Length; x++)
            {
                units[x].GatherResource(resource, destinations[x]);
            }
        }

    }
}
