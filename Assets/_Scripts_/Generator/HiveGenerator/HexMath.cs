//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
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


    // Position of hexagon center according to given coordinates,
    // coordinates determine the order of the hexagon in the grid.
    public static Vector3 Center(float hexSize, int x, int y)
    {
        Vector3 centrePosition;
        centrePosition.x = (x + y * 0.5f - y / 2) * (InnerRadius(hexSize) * 2f);
        centrePosition.y = y * (OuterRadius(hexSize) * 1.5f);
        centrePosition.z = 0f;
        return centrePosition;
    }

}
