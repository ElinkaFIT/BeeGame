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
        if (unitSelection == null) {
            Debug.Log("LOG: Error In UnitCommander");
        }

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
                else if (hit.collider.CompareTag("Room"))
                {
                    UnitsBuildRoom(hit.collider.GetComponent<Room>(), selectedUnits);

                }
                else if (hit.collider.CompareTag("Queen") || hit.collider.CompareTag("Nursery"))
                {
                    UnitsToWork(hit.collider.GetComponent<Room>(), selectedUnits);

                }
                else if (hit.collider.CompareTag("AIUnit"))
                {
                    UnitAI enemy = hit.collider.gameObject.GetComponent<UnitAI>();
                    UnitsAttackEnemy(enemy, selectedUnits);

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

    void UnitsBuildRoom(Room room, Unit[] units)
    {
        if (units.Length == 1)
        {
            units[0].BuildRoom(room, room.transform.position);
        }
        else
        {
            //for (int x = 0; x < units.Length; x++)
            //{
            //    units[x].BuildRoom(room, room.transform.position);
            //}

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
            // pro vsechny jednotky
            //for (int x = 0; x < units.Length; x++)
            //{
            //    units[x].BuildRoom(room, room.transform.position);
            //}

            units[0].WorkInRoom(room, room.transform.position);
        }
    }
}
