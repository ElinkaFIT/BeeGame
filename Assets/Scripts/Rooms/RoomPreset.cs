using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Building Preset", menuName = "New Building Preset")]
public class RoomPreset : ScriptableObject
{
    public int waxCost;
    public int propolisCost;
    public int costPerTurn;
    public GameObject prefab;
    public int population;
    public int jobs;
    public int food;
}
