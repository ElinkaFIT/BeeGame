using System.Collections;
using System.Collections.Generic;
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
            Vector3 position = new Vector3(Mathf.CeilToInt(mouse.x) - 1, Mathf.CeilToInt(mouse.y) - 1, 0);
            return position;
        }

    }


}
