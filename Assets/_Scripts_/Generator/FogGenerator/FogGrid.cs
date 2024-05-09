//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

/// <summary>
/// Generates a grid of fog tiles to cover the map, except for areas defined as the hive space.
/// </summary>
public class FogGrid : MonoBehaviour
{
    public int width;                           // Width of the grid in number of tiles.
    public int height;                          // Height of the grid in number of tiles.
    public ResourceTile tilePrefab;             // The prefab for the fog tile.

    /// <summary>
    /// Generate the grid of fog tiles at the start.
    /// </summary>
    private void Start()
    {
        GridGenerator();
    }

    /// <summary>
    /// Generates a grid of fog tiles based on specified width and height.
    /// </summary>
    void GridGenerator()
    {
        int startX = (int)transform.position.x; // Starting X position for the grid.
        int startY = (int)transform.position.y; // Starting Y position for the grid.

        // Determine the size of each tile to calculate positions correctly.
        int tileShiftX = (int)tilePrefab.gameObject.GetComponent<Transform>().localScale.x;
        int tileShiftY = (int)tilePrefab.gameObject.GetComponent<Transform>().localScale.y;

        // Iterate over each grid position and instantiate a tile if it's not within the hive space.
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int positionX = startX + x * tileShiftX;
                int positionY = startY + y * tileShiftY;

                // Check if the position is outside the designated "hive space" before placing a fog tile.
                if (!IsInHiveSpace(positionX, positionY))
                {
                    var newTile = Instantiate(tilePrefab, new Vector3(positionX, positionY), Quaternion.identity);
                }
            }
        }
    }

    /// <summary>
    /// Checks if a given position is within the designated hive space to avoid placing fog tiles there.
    /// </summary>
    /// <param name="posX">The X coordinate to check.</param>
    /// <param name="posY">The Y coordinate to check.</param>
    /// <returns>True if the position is within the hive space; otherwise, false.</returns>
    private bool IsInHiveSpace(int posX, int posY)
    {
        // Define bounds of the hive space and return true if the position is inside these bounds.
        if (-20 < posX && posX < 20)
        {
            if (-10 < posY && posY < 20)
            {
                return true;
            }
        }
        return false;
    }
}
