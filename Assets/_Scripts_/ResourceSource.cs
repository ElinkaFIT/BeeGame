using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ResourceType
{
    Nectar,
    Water,
    Wax,
    Pollen
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
