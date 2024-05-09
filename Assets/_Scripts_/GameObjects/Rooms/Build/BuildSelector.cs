//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the selection of build locations in the game, helping to identify the correct hexagonal tile.
/// </summary>
public class BuildSelector : MonoBehaviour
{
    private Camera cam;                      // Camera used to convert mouse position to world coordinates
    public static BuildSelector instance;    // Singleton instance of BuildSelector for global access

    /// <summary>
    /// Initializes the singleton instance of the BuildSelector.
    /// </summary>
    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Initializes the camera used to determine mouse position in the world.
    /// </summary>
    void Start()
    {
        cam = Camera.main;
    }

    /// <summary>
    /// Returns the current tile position under the mouse cursor, considering UI elements and game objects.
    /// </summary>
    /// <returns>The world position of the closest hexagonal tile to the mouse cursor.</returns>
    public Vector3 GetCurTilePosition()
    {
        // Check if the mouse pointer is over a UI element
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return new Vector3(0, 0, -99);
        }
        else
        {
            // Get the world position of the mouse cursor
            Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 position = new Vector3(mouse.x, mouse.y, 0);

            // Convert the array of all hexagons into a list for easier searching
            List<Vector3> hexList = HiveGenerator.hexagons.Cast<Vector3>().ToList();
            Vector3 closestHex = hexList[0];

            // Find the closest hex tile to the mouse position
            foreach (Vector3 hex in hexList)
            {
                if (Vector3.Distance(position, hex) < Vector3.Distance(position, closestHex))
                {
                    closestHex = hex;
                }
            }

            return closestHex;
        }
    }
}
