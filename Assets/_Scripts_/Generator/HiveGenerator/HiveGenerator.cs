//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;
using UnityEditor;

/// <summary>
/// Manages the generation of the hive grid using Perlin noise to simulate natural variations.
/// In the centre, they are almost certainly placed, and progressively less towards the edges.
/// </summary>
public class HiveGenerator : MonoBehaviour
{
    // Singleton instance of the HiveGenerator.
    public static HiveGenerator instance;

    // Parameters for grid generation
    [SerializeField] private int width;                     // Width of the hex grid.
    [SerializeField] private int height;                    // Height of the hex grid.
    [SerializeField] private Vector3 offset;                // Offset to apply to the grid positioning.
    [SerializeField] private float noiseLimit;              // Threshold for determining if a cell becomes a room.
    [SerializeField] private float noiseScale;              // Scale of the noise map.
    // Prefabs and settings for room generation
    [SerializeField] private RoomPreset curBuildingPreset;  // Current room preset used for generating rooms.

    // References to grid and room templates
    public Grid grid;                                       // Grid component reference for layout calculations.
    public GameObject hexEmpty;                             // Prefab for representing empty hex rooms.
    public List<GameObject> emptyRooms;                     // Collection of instantiated empty room objects.

    // Stores positions of hex centers
    public static Vector3[,] hexagons;                      // Stores the center positions of hex tiles.

    /// <summary>
    /// Initialize the singleton instance and grid component.
    /// </summary>
    private void Awake()
    {
        instance = this;
        grid = GetComponent<Grid>();
    }

    /// <summary>
    /// Generate the initial hex map and place the queen's tile.
    /// </summary>
    private void Start()
    {
        GenerateHexMap();
        AddQueenTile();
    }

    /// <summary>
    /// It was implemented for testing purposes
    /// </summary>
    private void Update()
    {
        // for testing regenerate on click

        //if (Input.GetMouseButtonDown(2))
        //{
        //    DeleteRooms();
        //    GenerateHexMap();
        //}
    }

    /// <summary>
    /// Generates a hexagonal map using Perlin noise to determine the placement of empty rooms.
    /// </summary>
    private void GenerateHexMap()
    {
        float hexSize = grid.cellSize.x;
        int xCenter = width / 2;
        int yCenter = height / 2;

        hexagons = new Vector3[width, height];

        float[,] noiseMap = CreatePerlinNoise();

        for (int y = 0; y < height; y++)
        {
            // Calculates the probability of generating a room -> y
            float yDistance = Math.Abs(y - yCenter);
            float yPercentage = 1 - (yDistance / yCenter);

            for (int x = 0; x < width; x++)
            {
                // Calculates the probability of generating a room -> x
                float xDistance = Math.Abs(x - xCenter);
                float xPercentage = 1 - (xDistance / xCenter);

                float noiseValue = noiseMap[x, y] + xPercentage + yPercentage;

                if (noiseValue > noiseLimit)
                {
                    Vector3 centrePosition = HexMath.Center(hexSize, x, y) + offset;
                    hexagons[x, y] = centrePosition;

                    Vector3 finalPosition = new Vector3(centrePosition.x, centrePosition.y, 0.1f);
                    GameObject newEmptyRoom = Instantiate(hexEmpty, finalPosition, Quaternion.identity);
                    emptyRooms.Add(newEmptyRoom);
                }
                else
                {
                    hexagons[x, y] = new Vector3(-999, -999, -999);
                }
            }
        }
    }

    /// <summary>
    /// Adds the queen's tile to the grid by placing it in the calculated center of the existing empty rooms.
    /// </summary>
    private void AddQueenTile()
    {
        float minX = emptyRooms[0].transform.position.x;
        float minY = emptyRooms[0].transform.position.y;
        float maxX = emptyRooms[0].transform.position.x;
        float maxY = emptyRooms[0].transform.position.y;

        foreach (GameObject room in emptyRooms)
        {
            if (room.transform.position.x < minX)
            {
                minX = room.transform.position.x;
            }
            else if (room.transform.position.x > maxX)
            {
                maxX = room.transform.position.x;
            }

            if (room.transform.position.y < minY)
            {
                minY = room.transform.position.y;
            }
            else if (room.transform.position.y > maxY)
            {
                maxY = room.transform.position.y;
            }
        }

        // Calculate center room
        Vector2 midPosition;
        midPosition.x = (minX + maxX) / 2;
        midPosition.y = (minY + maxY) / 2;

        // Create queen in center
        GameObject closest = emptyRooms[0];

        foreach (GameObject room in emptyRooms)
        {
            if (Vector2.Distance(room.transform.position, midPosition) < Vector2.Distance(closest.transform.position, midPosition))
            {
                closest = room;
            }
        }

        Hive.instance.OnPlaceBuilding(curBuildingPreset, closest.transform.position);
    }

    /// <summary>
    /// Creates a Perlin noise map to determine room placement.
    /// </summary>
    /// <returns>A 2D array of noise values.</returns>
    private float[,] CreatePerlinNoise()
    {
        float[,] noiseMap = new float[width, height];
        float xOffset = UnityEngine.Random.Range(-10000f, 10000f);
        float yOffset = UnityEngine.Random.Range(-10000f, 10000f);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * noiseScale + xOffset, y * noiseScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        return noiseMap;
    }

    /// <summary>
    /// Currently only for testing purposes
    /// </summary>
    private void DeleteRooms()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("EmptyRoom");
        emptyRooms.Clear();

        foreach (GameObject go in gameObjects)
        {
            Destroy(go);
        }
    }
}