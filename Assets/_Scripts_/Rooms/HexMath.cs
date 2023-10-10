using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public static class HexMath
{
    public static float OuterRadius (float hexSize)
    {
        return hexSize;
    }

    public static float InnerRadius (float hexSize)
    {
        return hexSize * (Mathf.Sqrt(3) / 2);   
    }

    public static Vector3[] Corners(float hexSize)
    {
        Vector3[] corners = new Vector3[6];
        for (int i = 0; i < 6; i++) 
        {
            // + 30f kvuli pointtop
            float angle = 60f * i + 30f;
            // jednotlivy bod
            corners[i] = new Vector3(hexSize * Mathf.Cos(angle * Mathf.Deg2Rad), hexSize * Mathf.Sin(angle * Mathf.Deg2Rad), 0f);
        }
        return corners;
    }

    public static Vector3 Center(float hexSize, int x, int y)
    {
        Vector3 centrePosition;
        centrePosition.x = (x + y * 0.5f - y / 2) * (InnerRadius(hexSize) * 2f);
        centrePosition.y = y * (OuterRadius(hexSize) * 1.5f);
        centrePosition.z = 0f;
        return centrePosition;
    }

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
