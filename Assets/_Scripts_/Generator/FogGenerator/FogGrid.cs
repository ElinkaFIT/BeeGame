using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FogGrid : MonoBehaviour
{
    public int width;
    public int height;

    public ResourceTile tilePrefab;

    private void Start()
    {
        GridGenerator();
    }
    void GridGenerator()
    {
        int startX = (int)transform.position.x;
        int startY = (int)transform.position.y;

        // ziska velikost skoku dle velikosti policka
        int tileShiftX = (int)tilePrefab.gameObject.GetComponent<Transform>().localScale.x;
        int tileShiftY = (int)tilePrefab.gameObject.GetComponent<Transform>().localScale.y;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                int positionX = startX + x * tileShiftX;
                int positionY = startY + y * tileShiftY;

                if (!IsInHiveSpace(positionX, positionY))
                {
                    var newTile = Instantiate(tilePrefab, new Vector3(positionX, positionY), Quaternion.identity);
                }
                 

            }
        }
    }

    private bool IsInHiveSpace(int posX, int posY)
    {
        if(-20 < posX && posX < 20)
        {
            if(-10 < posY && posY < 20)
            {
                return true;
            }
        }
        return false;
    } 

}
