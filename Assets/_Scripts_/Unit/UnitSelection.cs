using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public LayerMask unitLayerMask;
    private List<Unit> selectedUnits = new List<Unit>();

    // pro vyber pomoci boxu
    public RectTransform selectionBox;
    private Vector2 startPos;

    // komponenty
    private Camera cam;
    private Player player;

    void Awake()
    {
        cam = Camera.main;
        player = GetComponent<Player>();
    }

    void Update()
    {
        // na klik levym tlacitkem
        if (Input.GetMouseButtonDown(0))
        {
            ToggleSelectionVisual(false);
            selectedUnits = new List<Unit>();

            Select(Input.mousePosition);
            
            startPos = Input.mousePosition;
        }

        // odkliknu levym
        if (Input.GetMouseButtonUp(0))
        {
            ReleaseSelectionBox();
        }

        // drzenim tlacitka
        if (Input.GetMouseButton(0))
        {
            UpdateSelectionBox(Input.mousePosition);
        }
    }

    // vytvari spolecny vyber (box)
    void UpdateSelectionBox(Vector2 curMousePos)
    {
        // aktivuje box
        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        // velikost boxu
        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;

        // pozice boxu
        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }

    // vytvori vyber a vypne box
    void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);
        foreach (Unit unit in player.units)
        {
            Vector2 screenPos = cam.WorldToScreenPoint(unit.transform.position);

            if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
            {
                selectedUnits.Add(unit);
                unit.ToggleSelectionVisual(true);
            }
        }
    }

    // zobrazeni vyberu
    void ToggleSelectionVisual(bool selected)
    {
        foreach (Unit unit in selectedUnits)
        {
            unit.ToggleSelectionVisual(selected);
        }
    }

    // vyber jednotky
    void Select(Vector2 screenPos)
    {
        Vector2 origin = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
                                         Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.zero, 0f);

        // pokud klikne na jednotku
        if (hit)
        {
            Unit unit = hit.collider.GetComponent<Unit>();

            // nesmi byt nepratelska jednotka
            if (player.IsMyUnit(unit))
            {
                selectedUnits.Add(unit);
                unit.ToggleSelectionVisual(true);
            }
        }
    }
    public bool AreUnitsSelected()
    {
        return selectedUnits.Count > 0 ? true : false;
    }

    public Unit[] GetSelectedUnits()
    {
        return selectedUnits.ToArray();
    }

    // oddela mrtve jednotky
    public void RemoveNullUnitsFromSelection()
    {
        for (int x = 0; x < selectedUnits.Count; x++)
        {
            if (selectedUnits[x] == null)
                selectedUnits.RemoveAt(x);
        }
    }
}
