using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class BuildSelector : MonoBehaviour
{
    private Camera cam;
    public static BuildSelector instance;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        cam = Camera.main;
    }

    public Vector3 GetCurTilePosition()
    {
        // UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return new Vector3(0, -99, 0);
        }
        // spravna pozice
        else
        {
            Vector3 mouse = cam.ScreenToWorldPoint(Input.mousePosition);

            Vector3 position = new Vector3(mouse.x, mouse.y, 0);

            // convert array of all hexagons to list for easier searching
            List<Vector3> hexList = GridHex.hexagons.Cast<Vector3>().ToList();
            

            Vector3 closestHex = hexList[0];
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
