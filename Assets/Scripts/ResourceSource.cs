using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum ResourceType
{
    Food,
    Water
}


public class ResourceSource : MonoBehaviour
{
    public ResourceType type;
    public int quantity;

    // events
    public UnityEvent onQuantityChange;

    public void GatherResource(int amount, Player gatheringPlayer)
    {
        quantity -= amount;
        int amountToGive = amount;

        if (quantity < 0)
            amountToGive = amount + quantity;

        gatheringPlayer.GainResource(type, amountToGive);

        if (quantity <= 0)
            Destroy(gameObject);

        if (onQuantityChange != null)
            onQuantityChange.Invoke();
    }
}
