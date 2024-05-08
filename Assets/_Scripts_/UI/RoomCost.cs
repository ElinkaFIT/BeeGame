//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 
/// </summary>
public class RoomCost : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public RoomPreset preset;               // room preset
    public GameObject panel;                // panel for room information
    public TextMeshProUGUI textCost;        // text of room cost on panel
    public TextMeshProUGUI textDurability;  // text of room  durability on panel
    public TextMeshProUGUI textInfo;        // information text abour room

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);
        textCost.text = "Wax cost: " + preset.waxCost;
        textDurability.text = "Durability: " + preset.roomHealthMax;
        textInfo.text = preset.roomDescription;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }
}
