//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Represents the possible states of a resource tile.
/// </summary>
public enum ResourceTileState
{
    Fog,           // The tile is covered in fog.
    Discovering,   // The tile is in the process of being discovered.
    Exposed        // The tile has been fully discovered and is exposed.
}

/// <summary>
/// Manages the state and behavior of a resource tile.
/// </summary>
public class ResourceTile : MonoBehaviour
{
    public ResourceTileState state;             // Current state of the tile.
    public BuildProgressBar progressBar;        // Progress bar for revealing the tile.
    public int buildProgress;                   // Current progress of revealing the tile.
    public int buildModifier;                   // Modifier to speed up the build progress.

    public List<GameObject> resourceOptions;    // Possible resources that can appear on this tile.
    public TextMeshProUGUI dangerText;          // UI text that shows the danger level.
    public int dangerValue;                     // Numeric value indicating the danger level.
    public int difficultyModifier;              // Modifier that affects the overall difficulty.

    public int resourceCountMin;                // Minimum number of resources that can spawn on this tile.
    public int resourceCountMax;                // Maximum number of resources that can spawn on this tile.

    public bool isVertical;                     // Indicates if the tile is oriented vertically.

    private float posX;                         // X position of the tile.
    private float posY;                         // Y position of the tile.

    private float tileShiftX;                   // Horizontal shift used for resource placement.
    private float tileShiftY;                   // Vertical shift used for resource placement.

    /// <summary>
    /// Initializes the resource tile, sets its position, and calculates its danger.
    /// </summary>
    void Start()
    {
        posX = gameObject.transform.position.x;
        posY = gameObject.transform.position.y;

        tileShiftX = gameObject.GetComponent<Transform>().localScale.x;
        tileShiftY = gameObject.GetComponent<Transform>().localScale.y;

        GenerateTileDanger();
        SetResourceTileState(ResourceTileState.Fog);
    }

    /// <summary>
    /// Calculates the danger value based on the tile's distance from the hive center.
    /// </summary>
    private void GenerateTileDanger()
    {
        float distance = Vector2.Distance(new Vector2(posX, posY), new Vector2(0, 0));

        dangerValue = (int)distance / 2 + difficultyModifier;
        dangerText.text = "Danger " + dangerValue + " %";
    }

    /// <summary>
    /// Progresses the discovery of the tile and spawns resources if fully discovered.
    /// </summary>
    /// <param name="amount">Amount to increment the discovery progress.</param>
    public void SearchArea(int amount)
    {
        if (buildProgress >= 100)
        {
            progressBar.CloseProgressBar();
            SetResourceTileState(ResourceTileState.Exposed);
            if (resourceOptions != null)
            {
                if (isVertical)
                {
                    SpawnTree();
                }
                else
                {
                    SpawnResources();
                }
            }
            return;
        }

        buildProgress = buildProgress + buildModifier + amount;
        progressBar.UpdateProgressBar(buildProgress, 100);
    }

    /// <summary>
    /// Spawns resources on the tile based on its type and available options.
    /// </summary>
    private void SpawnResources()
    {
        // Randomly choose object which will be generated
        int resourceIndex = UnityEngine.Random.Range(0, resourceOptions.Count - 1);
        Debug.Log(resourceIndex);
        Debug.Log(resourceOptions[resourceIndex]);

        Vector2 startPos = new Vector2(posX - tileShiftX / 2, posY - tileShiftY / 2);

        // Randomly generate placement of resources
        int resourceCount = UnityEngine.Random.Range(resourceCountMin, resourceCountMax);
        List<Vector2> takenPositions = new List<Vector2>();

        for (int i = 0; i < resourceCount; i++)
        {
            Vector2 pos = new Vector2();

            float resourceSizeX = resourceOptions[resourceIndex].GetComponent<CapsuleCollider2D>().size.x;
            float resourceSizeY = resourceOptions[resourceIndex].GetComponent<CapsuleCollider2D>().size.y;

            pos.x = startPos.x + UnityEngine.Random.Range(0 + resourceSizeX, tileShiftX - resourceSizeX);
            pos.y = startPos.y + UnityEngine.Random.Range(0 + resourceSizeY, tileShiftY - resourceSizeY);

            // If resource is near new resource creation will be skipped
            bool resourceIsNear = false;

            foreach (Vector2 takenPos in takenPositions)
            {

                float biggerSize = resourceSizeX > resourceSizeY ? resourceSizeX : resourceSizeY;

                if (Vector2.Distance(pos, takenPos) < 2 * biggerSize)
                {
                    resourceIsNear = true;
                }
            }

            if (!resourceIsNear)
            {
                takenPositions.Add(pos);
                Instantiate(resourceOptions[resourceIndex], pos, Quaternion.identity);
            }

        }

        RemoveTile();
        RebuildNavMesh();
    }

    /// <summary>
    /// Spawns a tree-like structure if the tile is vertical.
    /// </summary>
    private void SpawnTree()
    {
        int resourceCount = UnityEngine.Random.Range(resourceCountMin, resourceCountMax);

        // Randomly choose object which will be generated
        int resourceIndex = UnityEngine.Random.Range(0, resourceOptions.Count - 1);

        for (int i = 0; i < resourceCount; i++)
        {
            // Start from bottom left corner
            Vector2 startPos = new Vector2(posX - tileShiftX / 2, posY - tileShiftY / 2);

            Vector2 pos = new Vector2();

            float resourceSizeX = resourceOptions[resourceIndex].GetComponent<CapsuleCollider2D>().size.x;
            float resourceSizeY = resourceOptions[resourceIndex].GetComponent<CapsuleCollider2D>().size.y;

            pos.x = startPos.x + UnityEngine.Random.Range(0 + resourceSizeX, tileShiftX - resourceSizeX);
            pos.y = startPos.y + resourceSizeY / 2;

            // Randomly generate resource placement
            Instantiate(resourceOptions[resourceIndex], pos, Quaternion.identity);
        }

        RemoveTile();
        RebuildNavMesh();
    }

    /// <summary>
    /// Currently unused: Placeholder for future functionality to rebuild navigation mesh.
    /// </summary>
    private void RebuildNavMesh()
    {
        //NavMeshBuilder.ClearAllNavMeshes();
        //NavMeshBuilder.BuildNavMesh();
    }


    /// <summary>
    /// Disables the tile and updates the navigation mesh.
    /// </summary>
    private void RemoveTile()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Sets the current state of the resource tile.
    /// </summary>
    /// <param name="toState">The new state to set.</param>
    public void SetResourceTileState(ResourceTileState toState)
    {
        state = toState;
    }
}
