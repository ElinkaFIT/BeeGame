//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

/// <summary>
/// Manages unit commands and interactions in the game world.
/// </summary>
public class UnitCommander : MonoBehaviour
{
    public LayerMask layerMask;                  // Layer mask to filter out which objects should be raycasted
    private Vector2 target;                      // Target location for moving units
    private UnitSelection unitSelection;         // UnitSelection unitSelection;
    private Camera cam;                          // Main camera used for converting mouse position

    /// <summary>
    /// Initializes the commander by obtaining necessary components.
    /// </summary>
    void Awake()
    {
        // Retrieve the UnitSelection component and the main camera
        unitSelection = GetComponent<UnitSelection>();
        cam = Camera.main;
    }

    /// <summary>
    /// Handles user input and updates unit commands each frame.
    /// </summary>
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

                // Send units to gather resources
                if (hit.collider.CompareTag("Resource"))
                {
                    UnitsGatherResource(hit.collider.GetComponent<ResourceSource>(), selectedUnits);
                }
                // Send units to search a fogged tile
                else if (hit.collider.CompareTag("FogTile"))
                {
                    UnitsSearching(hit.collider.GetComponent<ResourceTile>(), selectedUnits);
                }
                // Send units to build or work on a room
                else if (hit.collider.CompareTag("Room"))
                {
                    UnitsBuildRoom(hit.collider.GetComponent<Room>(), selectedUnits);
                }
                // Send units to work in a specific room
                else if (hit.collider.CompareTag("Queen") || hit.collider.CompareTag("Nursery")
                    || hit.collider.CompareTag("RestRoom") || hit.collider.CompareTag("FoodRoom")
                    || hit.collider.CompareTag("HoneyFactory") || hit.collider.CompareTag("WaxFactory"))
                {
                    UnitsToWork(hit.collider.GetComponent<Room>(), selectedUnits);
                }
                // Send units to attack an enemy
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

    /// <summary>
    /// Commands units to attack a specific enemy.
    /// </summary>
    /// <param name="target">The enemy to attack.</param>
    /// <param name="units">The units that will execute the attack.</param>
    void UnitsAttackEnemy(UnitAI target, Unit[] units)
    {
        for (int x = 0; x < units.Length; x++)
        {
            units[x].AttackUnit(target);
        }
    }

    /// <summary>
    /// Commands units to move to a specified position.
    /// </summary>
    /// <param name="target">The target position for the units to move.</param>
    /// <param name="units">The units to move.</param>
    void UnitsMoveToPosition(Vector2 target, Unit[] units)
    {
        // Calculate precise positions for the units to maintain formation
        Vector2[] destination = GetUnitPosition(target, units.Length, 1);
        for (int x = 0; x < units.Length; x++)
        {
            units[x].MoveToPosition(destination[x]);
        }
    }

    /// <summary>
    /// Commands units to gather resources from a specified source.
    /// </summary>
    /// <param name="resource">The resource source to gather from.</param>
    /// <param name="units">The units that will gather the resources.</param>
    void UnitsGatherResource(ResourceSource resource, Unit[] units)
    {
        Vector2[] destinations = GetUnitPosition(resource.transform.position, units.Length, 1);
        for (int x = 0; x < units.Length; x++)
        {
            units[x].GatherResource(resource, destinations[x]);
        }
    }

    /// <summary>
    /// Calculates a formation for units around a target position.
    /// </summary>
    /// <param name="moveToPos">The central position to move to.</param>
    /// <param name="numUnits">The number of units that need positions.</param>
    /// <param name="unitGap">The gap between units in the formation.</param>
    /// <returns>An array of positions for each unit.</returns>
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

    /// <summary>
    /// Commands units to search a specific resource tile.
    /// </summary>
    /// <param name="tile">The resource tile to be searched.</param>
    /// <param name="units">The units that will perform the search.</param>
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

    /// <summary>
    /// Commands units to build a room.
    /// </summary>
    /// <param name="room">The room where the work will happen.</param>
    /// <param name="units">The units that will buildk.</param>
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

    /// <summary>
    /// Commands units to work in a specific room.
    /// </summary>
    /// <param name="room">The room to work in.</param>
    /// <param name="units">The units that will work.</param>
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
