//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

/// <summary>
/// Defines the possible types of rooms that can be built within the game.
/// </summary>
public enum RoomType
{
    Queen,          // Room designated for the queen bee.
    Nursery,        // Room used to grow new bees.
    Warehouse,      // Storage room for additional resources.
    RestRoom,       // Room where bees can rest and recover energy.
    FoodRoom,       // Room for processing and storing food.
    HoneyFactory,   // Facility to produce honey.
    WaxFactory,     // Facility to produce wax.
    Defensive       // Room used for defensive purposes.
}

/// <summary>
/// Defines a scriptable object that holds the preset configurations for a room.
/// </summary>
[CreateAssetMenu(fileName = "Building Preset", menuName = "New Building Preset")]
public class RoomPreset : ScriptableObject
{
    public RoomType roomType;            // Type of the room as defined in RoomType.
    public GameObject prefab;            // Prefab used to visually represent the room in the game.

    public int waxCost;                  // Cost in wax to build the room.
    public int costPerTurn;              // Ongoing cost per turn to maintain the room.
    public string roomDescription;       // Description of the room's purpose and features.
    public int roomHealthMax;            // Maximum health points of the room.
    public int workersCapacity;          // Maximum number of workers that can operate in the room.