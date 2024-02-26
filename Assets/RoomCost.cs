using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomCost : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    public RoomPreset preset;
    public GameObject panel;
    public TextMeshProUGUI textMeshPro;
    public void OnPointerEnter(PointerEventData eventData)
    {
        panel.SetActive(true);
        textMeshPro.text = " Wax cost: " + preset.waxCost;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        panel.SetActive(false);
    }
}
