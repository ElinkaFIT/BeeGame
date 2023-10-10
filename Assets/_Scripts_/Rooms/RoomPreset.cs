using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public enum RoomType
{
    Test,
    House,
    Queen,
    Nursery,
    Warehouse
}

[CreateAssetMenu(fileName = "Building Preset", menuName = "New Building Preset")]

public class RoomPreset : ScriptableObject
{
    public RoomType roomType;
    public int waxCost;
    public int propolisCost;
    public int costPerTurn;
    public GameObject prefab;
    public int population;
    public int jobs;
    public int food;
}
