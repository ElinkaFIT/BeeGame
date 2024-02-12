using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAI : MonoBehaviour
{
    public float minSpawnRate;
    public float maxSpawnRate;

    [Header("Units")]
    public List<UnitAI> units = new List<UnitAI>();

    public GameObject unitPrefab;


    void Start()
    {
        InvokeRepeating("SpawnUnit", 0.0f, Random.Range(minSpawnRate, maxSpawnRate));
    }

    void Awake()
    {
        
    }

    void Update()
    {
        GameUI.instance.UpdateEnemyText(units.Count);
    }


    // called when a new unit is created
    public void SpawnUnit()
    {
        Vector3 spawnPos = new Vector3(5, -3, 0);
        GameObject unitObj = Instantiate(unitPrefab, spawnPos, Quaternion.identity, transform);
        UnitAI unit = unitObj.GetComponent<UnitAI>();
        units.Add(unit);
    }

}
