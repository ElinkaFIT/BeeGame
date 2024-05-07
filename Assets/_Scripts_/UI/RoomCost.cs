//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomCost : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public RoomPreset preset;
    public GameObject panel;
    public TextMeshProUGUI textCost;
    public TextMeshProUGUI textDurability;
    public TextMeshProUGUI textInfo;
    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);
        textCost.text = "Wax cost: " + preset.waxCost;
        textDurability.text = "Durability: " + preset.roomHealthMax;
        textInfo.text = preset.roomDescription;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }
}
