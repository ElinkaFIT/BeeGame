//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    public static PlayerAI enemy;

    public float minSpawnRate;
    public float maxSpawnRate;

    [Header("Units")]
    public List<UnitAI> units = new List<UnitAI>();

    public GameObject unitPrefab;
    public GameObject hiveUnitPrefab;

    public CommandManager commandManager;


    void Start()
    {
        enemy = this;

        // how to add add command
        //commandManager.AddSpawnEnemy();
        //commandManager.AddSpawnHiveEnemy();
    }

    void Update()
    {
        GameUI.instance.UpdateEnemyText(units.Count);
    }

    public bool IsMyUnit(UnitAI unit)
    {
        return units.Contains(unit);

    }

    // called when a new unit is created
    public void SpawnUnit()
    {
        Vector3 spawnPos = new(5, -3, 0);
        GameObject unitObj = Instantiate(unitPrefab, spawnPos, Quaternion.identity);

        UnitAI unit = unitObj.GetComponent<UnitAI>();
        units.Add(unit);
    }

    public void SpawnHiveUnit()
    {
        Vector3 spawnPos = new(-20, -20, 0);
        GameObject unitObj = Instantiate(hiveUnitPrefab, spawnPos, Quaternion.identity);

        UnitAI unit = unitObj.GetComponent<UnitAI>();
        units.Add(unit);
    }
}
