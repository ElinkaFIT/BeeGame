using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomCost : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public RoomPreset preset;
    public GameObject panel;
    public TextMeshProUGUI textCost;
    public TextMeshProUGUI textDurability;
    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);
        textCost.text = "Wax cost: " + preset.waxCost;
        textDurability.text = "Durability: " + preset.roomHealthMax;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }
}
