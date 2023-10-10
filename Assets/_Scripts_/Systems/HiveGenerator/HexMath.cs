using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

/// <summary>
/// Mathematical functions for determining the properties of a flat-top oriented hexagons
/// </summary>
public static class HexMath
{
    // Lenght from left to right corner
    public static float OuterRadius (float hexSize)
    {
        return hexSize;
    }


    // Height from bottom to top edge 
    public static float InnerRadius (float hexSize)
    {
        return (hexSize / 2) * Mathf.Sqrt(3);   
    }


    public static Vector3 Center(float hexSize, int x, int y)
    {
        Vector3 centrePosition;
        centrePosition.x = (x + y * 0.5f - y / 2) * (InnerRadius(hexSize) * 2f);
        centrePosition.y = y * (OuterRadius(hexSize) * 1.5f);
        centrePosition.z = 0f;
        return centrePosition;
    }


    // Find the positions of the six corners of the hexagon
    //public static Vector3[] Corners(float hexSize)
    //{
    //    Vector3[] corners = new Vector3[6];
    //    for (int i = 0; i < 6; i++) 
    //    {
    //        // added 30f becouse hexagon is flat-top
    //        float angle = 60f * i + 30f;

    //        corners[i] = new Vector3(hexSize * Mathf.Cos(angle * Mathf.Deg2Rad), hexSize * Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
    //    }
    //    return corners;
    //}


    // Find the positions of the six neighbors of the hexagon
    //public static List<Vector3> GetNeighbors(float hexSize, Vector3 middlePosition)
    //{
    //    float outer = OuterRadius(hexSize);
    //    float inner = InnerRadius(hexSize);

    //    List<Vector3> neighbors = new List<Vector3>
    //    {
    //        new Vector3(middlePosition.x + inner * 2f, middlePosition.y, middlePosition.z),
    //        new Vector3(middlePosition.x - inner * 2f, middlePosition.y, middlePosition.z),

    //        new Vector3(middlePosition.x + inner, middlePosition.y + outer * 1.5f, middlePosition.z),
    //        new Vector3(middlePosition.x + inner, middlePosition.y - outer * 1.5f, middlePosition.z),

    //        new Vector3(middlePosition.x -  inner, middlePosition.y + outer * 1.5f, middlePosition.z),
    //        new Vector3(middlePosition.x -  inner, middlePosition.y - outer * 1.5f, middlePosition.z)
    //    };

    //    return neighbors;
    //}

}
