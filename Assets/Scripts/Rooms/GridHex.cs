using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GridHex : MonoBehaviour
{
    public Grid grid;
    public static Vector3[,] hexagons; // dvourozmerne pole uchovavajici stredy hexagonu


    [Header("Info")]
    public int width;
    public int height;
    public Vector3 offset;
    public float noiseLimit;
    public float noiseScale;

    public GameObject hexEmpty;




    private void Awake()
    {
        grid = GetComponent<Grid>();
        CreateAllHexCenter();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            DeleteRooms();
            CreateAllHexCenter();
        }
    }

    public void CreateAllHexCenter()
    {

        int hexSize = (int)grid.cellSize.x;
        int xCenter = width / 2;
        int yCenter = height / 2;
        hexagons = new Vector3[width, height];

        float[,] noiseMap = CreatePerlinNoise();

        for (int y = 0; y < height; y++)
        {
            // spocita pravdepodobnost vygenerovani policka pro y
            float yDistance = Math.Abs(y - yCenter);
            float yPercentage = 1 - (yDistance / yCenter);


            for (int x = 0; x < width; x++)
            {
                // spocita pravdepodobnost vygenerovani policka pro x
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
                    hexagons[x, y] = new Vector3(0, 0, -99);
                }

            }
        }

    }

    public float[,] CreatePerlinNoise()
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

    public void DeleteRooms()
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("EmptyRoom");

        foreach (GameObject go in gameObjects)
        {
            Destroy(go);
        }
    }

}
 