using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class PlayerAI : MonoBehaviour
{
    public float minSpawnRate = 1.0f;
    public float maxSpawnRate = 1.0f;

    [Header("Units")]
    public List<Unit> units = new List<Unit>();

    private Player player;

    void Start()
    {
        InvokeRepeating("SpawnUnit", 0.0f, Random.Range(minSpawnRate, maxSpawnRate + 1));
    }

    void Awake()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        GameUI.instance.UpdateEnemyText(units.Count);
    }



    // called when a new unit is created
    public void SpawnUnit()
    {
        
    }

}
