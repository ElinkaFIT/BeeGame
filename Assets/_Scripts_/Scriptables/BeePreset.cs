using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Unit Preset", menuName = "New Unit Preset")]

public class BeePreset : ScriptableObject
{
    [Header("Stats")]

    [SerializeField] private int health;
    [SerializeField] private int damage;
    [SerializeField] private int energy;
    [SerializeField] private int gatherRate;
    [SerializeField] private int buildRate;
    [SerializeField] private int nurseRate;
    [SerializeField] private int makerRate;

}
