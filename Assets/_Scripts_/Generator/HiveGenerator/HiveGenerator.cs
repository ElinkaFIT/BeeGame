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
/// Generating empty hive rooms using perlin noise.
/// In the centre, they are almost certainly placed, and progressively less towards the edges.
/// The room objects here are for visual purposes only.
/// </summary>
public class HiveGenerator : MonoBehaviour
{
    public static HiveGenerator instance;

    // serialized field
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float noiseLimit;
    [SerializeField] private float noiseScale;
    [SerializeField] private RoomPreset curBuildingPreset;

    // component references
    public Grid grid;
    public GameObject hexEmpty;
    public List<GameObject> emptyRooms;

    // other references
    public static Vector3[,] hexagons; // dvourozmerne pole uchovavajici stredy hexagonu


    private void Awake()
    {
        instance = this;
        grid = GetComponent<Grid>();
    }

    private void Start()
    {
        GenerateHexMap();
        AddQueenTile();
    }

    private void Update()
    {
        // jen pro testovani
        //if (Input.GetMouseButtonDown(2))
        //{
        //    DeleteRooms();
        //    GenerateHexMap();
        //}
    }

    // Generate empty rooms to scene according to the specified limit and offset
    private void GenerateHexMap()
    {
        float hexSize = grid.cellSize.x;
        int xCenter = width / 2;
        int yCenter = height / 2;

        hexagons = new Vector3[width, height];

        float[,] noiseMap = CreatePerlinNoise();

        for (int y = 0; y < height; y++)
        {
            // calculates the probability of generating a room -> y
            float yDistance = Math.Abs(y - yCenter);
            float yPercentage = 1 - (yDistance / yCenter);

            for (int x = 0; x < width; x++)
            {
                // calculates the probability of generating a room -> x
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

    private void AddQueenTile()
    {
        // vypocitej krajni pozice ulu
        float minX = emptyRooms[0].transform.position.x;
        float minY = emptyRooms[0].transform.position.y;
        float maxX = emptyRooms[0].transform.position.x;
        float maxY = emptyRooms[0].transform.position.y;

        foreach(GameObject room in emptyRooms)
        {
            if (room.transform.position.x < minX)
            {
                minX = room.transform.position.x;
            }
            else if(room.transform.position.x > maxX)
            {
                maxX = room.transform.position.x;
            }

            if(room.transform.position.y < minY)
            {
                minY = room.transform.position.y;
            }
            else if(room.transform.position.y > maxY)
            {
                maxY = room.transform.position.y;
            }
        }

        // vypocitej prostredni pole
        Vector2 midPosition;
        midPosition.x = (minX + maxX) / 2;
        midPosition.y = (minY + maxY) / 2;

        // vytvor kralovnu ve stredu
        GameObject closest = emptyRooms[0];

        foreach(GameObject room in emptyRooms)
        {
            if(Vector2.Distance(room.transform.position, midPosition) < Vector2.Distance(closest.transform.position, midPosition))
            {
                closest = room;
            }
        }

        Hive.instance.OnPlaceBuilding(curBuildingPreset, closest.transform.position);

    }

    // Calculates random perlin noise and resizes it according to scale
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

    // Zatim jen pro testovani
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
 