using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    // serazeni jednotek do formace
    public static Vector2[] GetUnitGroupDestinations(Vector2 moveToPos, int numUnits, float unitGap)
    {
        Vector2[] destinations = new Vector2[numUnits];

        int rows = Mathf.RoundToInt(Mathf.Sqrt(numUnits));
        int cols = Mathf.CeilToInt((float)numUnits / (float)rows);
        
        int curRow = 0;
        int curCol = 0;
        float width = ((float)rows - 1) * unitGap;
        float length = ((float)cols - 1) * unitGap;
        for (int x = 0; x < numUnits; x++)
        {
            destinations[x] = moveToPos + (new Vector2(curRow, curCol) * unitGap) - new Vector2(length / 2, width / 2);
            curCol++;
            if (curCol == rows)
            {
                curCol = 0;
                curRow++;
            }
        }
        return destinations;
    }

    public static Vector2[] GetUnitGroupDestinationsAroundResource(Vector2 resourcePos, int unitsNum)
    {
        Vector2[] destinations = new Vector2[unitsNum]; 
        float unitDistanceGap = 360.0f / (float)unitsNum;
        for (int x = 0; x < unitsNum; x++)
        {
            float angle = unitDistanceGap * x;
            Vector2 dir = new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
            destinations[x] = resourcePos + dir;
        }
        return destinations;
    }

    public static Vector3 GetUnitDestinationAroundResource(Vector3 resourcePos)
    {
        float angle = Random.Range(0, 360);
        Vector3 dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0);
        return resourcePos;
    }

    public static Vector3[] GetUnitGroupDestinationsAroundResource(Vector3 resourcePos, int unitsNum)
    {
        Vector3[] destinations = new Vector3[unitsNum];
        float unitDistanceGap = 360.0f / (float)unitsNum;

        for (int x = 0; x < unitsNum; x++)
        {
            float angle = unitDistanceGap * x;
            Vector3 dir = new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad), 0);
            destinations[x] = resourcePos + dir;
        }
        return destinations;
    }



}
