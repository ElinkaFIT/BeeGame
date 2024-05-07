//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

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
    public string roomDescription;
    public int roomHealthMax;
    public int workersCapacity;
}
