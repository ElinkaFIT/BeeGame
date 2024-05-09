//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Between the Flowers
// Date:        09/05/2024
//****************************************************************************
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Displays room cost and other details when the mouse pointer enters the room's UI area.
/// </summary>
public class RoomCost : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public RoomPreset preset;               // Reference to the room preset containing cost and other properties.
    public GameObject panel;                // UI panel that displays the room's cost information.
    public TextMeshProUGUI textCost;        // Text component to display the wax cost of the room.
    public TextMeshProUGUI textDurability;  // Text component to display the room's maximum durability.
    public TextMeshProUGUI textInfo;        // Text component to display detailed information about the room.

    /// <summary>
    /// Shows the room's cost information panel when the mouse pointer enters the UI element.
    /// </summary>
    /// <param name="eventData">Event data associated with the pointer enter event.</param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);                                          // Activate the information panel.
        textCost.text = "Wax cost: " + preset.waxCost;                  // Display the wax cost.
        textDurability.text = "Durability: " + preset.roomHealthMax;    // Display the room's durability.
        textInfo.text = preset.roomDescription;                         // Display the room's description.
    }

    /// <summary>
    /// Hides the room's cost information panel when the mouse pointer exits the UI element.
    /// </summary>
    /// <param name="eventData">Event data associated with the pointer exit event.</param>
    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false); 
    }
}
