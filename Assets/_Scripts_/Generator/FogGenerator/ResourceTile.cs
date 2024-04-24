using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor.AI;
using UnityEngine;
using UnityEngine.UIElements;

public enum ResourceTileState
{
    Fog,
    Discovering,
    Exposed
}

public class ResourceTile : MonoBehaviour
{

    public ResourceTileState state;
    public BuildProgressBar progressBar;

    public int buildProgress;
    public int buildModifier;

    public List<GameObject> resourceOptions;

    public TextMeshProUGUI dangerText;
    public int dangerValue;
    public int difficultyModifier;

    public int resourceCountMin;
    public int resourceCountMax;

    public bool isVertical;

    private float posX;
    private float posY;

    private float tileShiftX;
    private float tileShiftY;

    // Start is called before the first frame update
    void Start()
    {
        posX = gameObject.transform.position.x;
        posY = gameObject.transform.position.y;

        tileShiftX = gameObject.GetComponent<Transform>().localScale.x;
        tileShiftY = gameObject.GetComponent<Transform>().localScale.y;

        GenerateTileDanger();

        SetResourceTileState(ResourceTileState.Fog);
    }

    // vygeneruje nebezpecnost policka dle vzdalenosti od ulu
    private void GenerateTileDanger()
    {
        float distance = Vector2.Distance(new Vector2(posX, posY), new Vector2(0,0));

        dangerValue = (int)distance / 2 + difficultyModifier;
        dangerText.text = "Danger " + dangerValue + " %";
    }

    public void SearchArea(int amount)
    {
        if (buildProgress >= 100)
        {
            progressBar.CloseProgressBar();
            SetResourceTileState(ResourceTileState.Exposed);
            if (resourceOptions != null)
            {
                SpawnResources();
            }
            return;
        }

        buildProgress = buildProgress + buildModifier + amount;
        progressBar.UpdateProgressBar(buildProgress, 100);

    }

    private void SpawnResources()
    {
        // nahodne vybere jaky typ objektu se bude na policku generovat
        int resourceIndex = UnityEngine.Random.Range(0, resourceOptions.Count);

        // zacneme z leveho horniho rohu
        Vector2 startPos = new Vector2(posX - tileShiftX / 2, posY - tileShiftY / 2);
        
        // nahodne generuje umisteni suroviny
        int resourceCount = UnityEngine.Random.Range(resourceCountMin, resourceCountMax);
        List<Vector2> takenPositions = new List<Vector2>();

        for (int i = 0; i < resourceCount; i++)
        {
            Vector2 pos = new Vector2();

            float resourceSizeX = resourceOptions[resourceIndex].GetComponent<CapsuleCollider2D>().size.x;
            float resourceSizeY = resourceOptions[resourceIndex].GetComponent<CapsuleCollider2D>().size.y;

            pos.x = startPos.x + UnityEngine.Random.Range(0 + resourceSizeX, tileShiftX - resourceSizeX);
            pos.y = startPos.y + UnityEngine.Random.Range(0 + resourceSizeY, tileShiftY - resourceSizeY);

            // pokud je prilis blizko jiz existujici surovine, nova nevznikne
            bool resourceIsNear = false;

            foreach (Vector2 takenPos in takenPositions) { 

                float biggerSize = resourceSizeX > resourceSizeY ? resourceSizeX : resourceSizeY;

                if (Vector2.Distance(pos, takenPos) < 2 * biggerSize) { 
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

    private void RebuildNavMesh()
    {
        //NavMeshBuilder.ClearAllNavMeshes();
        //NavMeshBuilder.BuildNavMesh();
    }

    private void RemoveTile()
    {
        gameObject.SetActive(false);
    }

    public void SetResourceTileState(ResourceTileState toState)
    {
        state = toState;
    }
}
