using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Generating empty hive rooms using perlin noise.
/// In the centre, they are almost certainly placed, and progressively less towards the edges.
/// The room objects here are for visual purposes only.
/// </summary>
public class GridHex : MonoBehaviour
{
    public static GridHex instance;

    // serialized field
    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float noiseLimit;
    [SerializeField] private float noiseScale;

    // component references
    public Grid grid;
    public GameObject hexEmpty;

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
    }

    private void Update()
    {
        // jen pro testovani
        if (Input.GetMouseButtonDown(2))
        {
            DeleteRooms();
            GenerateHexMap();
        }
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

                    Instantiate(hexEmpty, hexagons[x, y], Quaternion.identity);
                }
                else
                {
                    hexagons[x, y] = new Vector3(-999, -999, -999);
                }

            }
        }

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

        foreach (GameObject go in gameObjects)
        {
            Destroy(go);
        }
    }

    // Ziska sousedici hexagony
    //public List<Vector3> GetNeighbors2(Vector3 middlePosition)
    //{
    //    float hexSize = grid.cellSize.x;

    //    List<Vector3> neighbors = HexMath.GetNeighbors(hexSize, middlePosition);

    //    return neighbors;
    //}
}
 