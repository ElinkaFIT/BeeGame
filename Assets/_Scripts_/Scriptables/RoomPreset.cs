using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum RoomType
{
    Queen,
    Nursery,
    Warehouse,
    RestRoom,
    FoodRoom,
    HoneyFactory,
    WaxFactory,
    Defensive
}

[CreateAssetMenu(fileName = "Building Preset", menuName = "New Building Preset")]

public class RoomPreset : ScriptableObject
{
    public RoomType roomType;
    public GameObject prefab;

    public int waxCost;
    public int costPerTurn;
    public int roomHealthMax;
    public int workersCapacity;
}
