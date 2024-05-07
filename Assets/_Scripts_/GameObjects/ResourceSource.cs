//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.Events;

public enum ResourceType
{
    Nectar,
    Water,
    Wax,
    Pollen,
    Honey
}


public class ResourceSource : MonoBehaviour
{
    public ResourceType type;
    public int quantity;


    // events
    public UnityEvent onQuantityChange;

    public void GatherResource(int amount)
    {
        quantity -= amount;
        int amountToGive = amount;

        if (quantity < 0)
            amountToGive = amount + quantity;

        Hive.instance.GainResource(type, amountToGive);

        if (quantity <= 0)
            Destroy(gameObject);

        if (onQuantityChange != null)
            onQuantityChange.Invoke();
    }
}
