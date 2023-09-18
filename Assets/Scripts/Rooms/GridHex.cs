using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridHex : MonoBehaviour
{
    public Grid grid;
    public static Vector3[,] hexagons; // dvourozmerne pole uchovavajici stredy hexagonu


    [Header("Info")]
    public int width;
    public int height;
    public Vector3 offset;


    private void Awake()
    {
        grid = GetComponent<Grid>();
        CreateAllHexCenter();
    }

    public void CreateAllHexCenter()
    {
        int hexSize = (int)grid.cellSize.x;
        hexagons = new Vector3[height, width];

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                Vector3 centrePosition = HexMath.Center(hexSize, x, y);

                hexagons[x, y] = centrePosition - offset;
            }
        }
    }

    // mozna muzu zkratit nazaklade toho ze to poscitam uz ve funkci nahore
    private void OnDrawGizmos()
    {
        int hexSize = (int)grid.cellSize.x;

        for (int y = 0; y < width; y++)
        {
            for (int x = 0; x < height; x++)
            {
                Vector3 centrePosition = HexMath.Center(hexSize, x, y);

                for (int s = 0; s < HexMath.Corners(hexSize).Length; s++)
                {
                    // vykreslim cary z rohu do rohu
                    Gizmos.DrawLine(
                        centrePosition - offset + HexMath.Corners(hexSize)[s % 6],
                        centrePosition - offset + HexMath.Corners(hexSize)[(s + 1) % 6]
                        );

                }
            }
        }
    }

}
 