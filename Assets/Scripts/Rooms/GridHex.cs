using System;
using System.Collections;
using System.Collections.Generic;
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
    public float noiseLevel;
    public float noiseScale;

    public GameObject hexEmpty;



    private void Awake()
    {
        grid = GetComponent<Grid>();
        CreateAllHexCenter();
    }

    public void CreateAllHexCenter()
    {
        float[,] noiseMap = new float[width, height];
        float xOffset = UnityEngine.Random.Range(-10000f, 10000f);
        float yOffset = UnityEngine.Random.Range(-10000f, 10000f);

        int hexSize = (int)grid.cellSize.x;
        hexagons = new Vector3[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float noiseValue = Mathf.PerlinNoise(x * noiseScale + xOffset, y * noiseScale + yOffset);
                noiseMap[x, y] = noiseValue;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float noiseValue = noiseMap[x, y];

                if (noiseValue > noiseLevel)
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

}
 