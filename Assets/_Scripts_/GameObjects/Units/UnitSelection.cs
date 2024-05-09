//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the selection of units via mouse interactions.
/// </summary>
public class UnitSelection : MonoBehaviour
{
    public LayerMask unitLayerMask;                         // Layer mask to filter selectable units.
    public List<Unit> selectedUnits = new List<Unit>();     // List of currently selected units.
    public RectTransform selectionBox;                      // UI rectangle to show the selection box.
    private Vector2 startPos;                               // Start position of the selection box.
    private Camera cam;                                     // Main camera component.
    private Player player;                                  // Player component.
    public static UnitSelection instance;                   // Singleton instance of UnitSelection.

    /// <summary>
    /// Initializes the singleton instance, camera, and player reference.
    /// </summary>
    void Awake()
    {
        instance = this;
        cam = Camera.main;
        player = GetComponent<Player>();
    }

    /// <summary>
    /// Handles input for starting, updating, and completing unit selections.
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Start a new selection.
            ToggleSelectionVisual(false);
            selectedUnits = new List<Unit>();

            // Select single unit or start box selection.
            Select(Input.mousePosition);
            startPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            // Finalize box selection.
            ReleaseSelectionBox();
        }

        if (Input.GetMouseButton(0))
        {
            // Update the visual selection box.
            UpdateSelectionBox(Input.mousePosition);
        }
    }

    /// <summary>
    /// Updates the size and position of the selection box.
    /// </summary>
    /// <param name="curMousePos">Current mouse position.</param>
    void UpdateSelectionBox(Vector2 curMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
            selectionBox.gameObject.SetActive(true);

        float width = curMousePos.x - startPos.x;
        float height = curMousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);
    }

    /// <summary>
    /// Finalizes the selection process and selects units within the box.
    /// </summary>
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

    /// <summary>
    /// Toggles the selection visual for all selected units.
    /// </summary>
    /// <param name="selected">Whether to show or hide the selection visual.</param>
    void ToggleSelectionVisual(bool selected)
    {
        foreach (Unit unit in selectedUnits)
        {
            unit.ToggleSelectionVisual(selected);
        }
    }

    /// <summary>
    /// Selects a single unit or starts the selection box.
    /// </summary>
    /// <param name="screenPos">Screen position to check for unit selection.</param>
    void Select(Vector2 screenPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(cam.ScreenToWorldPoint(screenPos), Vector2.zero, 0f, unitLayerMask);

        if (hit)
        {
            Unit unit = hit.collider.GetComponent<Unit>();
            if (player.IsMyUnit(unit))
            {
                selectedUnits.Add(unit);
                unit.ToggleSelectionVisual(true);
            }
        }
    }

    /// <summary>
    /// Checks if any units are currently selected.
    /// </summary>
    /// <returns>True if one or more units are selected, false otherwise.</returns>
    public bool AreUnitsSelected()
    {
        return selectedUnits.Count > 0;
    }

    /// <summary>
    /// Gets the currently selected units as an array.
    /// </summary>
    /// <returns>Array of selected units.</returns>
    public Unit[] GetSelectedUnits()
    {
        return selectedUnits.ToArray();
    }

    /// <summary>
    /// Removes any null references from the selection list.
    /// </summary>
    public void RemoveNullUnitsFromSelection()
    {
        selectedUnits.RemoveAll(unit => unit == null);
    }
}
