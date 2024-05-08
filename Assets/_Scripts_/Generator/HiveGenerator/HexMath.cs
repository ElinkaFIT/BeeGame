//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//
// This script is based by https://www.redblobgames.com/grids/hexagons/
//****************************************************************************
using UnityEngine;

/// <summary>
/// Mathematical functions for determining the properties of a flat-top oriented hexagons
/// </summary>
public static class HexMath
{
    /// <summary>
    /// Lenght from left to right corner
    /// </summary>
    public static float OuterRadius (float hexSize)
    {
        return hexSize;
    }

    /// <summary>
    /// Height from bottom to top edge 
    /// </summary>
    public static float InnerRadius (float hexSize)
    {
        return (hexSize / 2) * Mathf.Sqrt(3);   
    }

    /// <summary>
    /// Position of hexagon center according to given coordinates, coordinates determine the order of the hexagon in the grid.
    /// </summary>
    public static Vector3 Center(float hexSize, int x, int y)
    {
        Vector3 centrePosition;
        centrePosition.x = (x + y * 0.5f - y / 2) * (InnerRadius(hexSize) * 2f);
        centrePosition.y = y * (OuterRadius(hexSize) * 1.5f);
        centrePosition.z = 0f;
        return centrePosition;
    }

}
