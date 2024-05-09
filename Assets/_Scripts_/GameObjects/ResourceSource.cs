//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Enumerates types of resources available in the game.
/// </summary>
public enum ResourceType
{
    Nectar, // Represents nectar as a resource.
    Water,  // Represents water as a resource.
    Wax,    // Represents wax as a resource.
    Pollen, // Represents pollen as a resource.
    Honey   // Represents honey as a resource.
}

/// <summary>
/// Represents a source of resources that units can gather.
/// </summary>
public class ResourceSource : MonoBehaviour
{
    public ResourceType type;                       // The type of resource this source provides.
    public int quantity;                            // The current quantity of the resource available.
    public UnityEvent onQuantityChange;             //Event triggered when the quantity of the resource changes.

    /// <summary>
    /// Reduces the quantity of the resource based on the amount gathered.
    /// </summary>
    /// <param name="amount">The amount of resource units attempt to gather.</param>
    public void GatherResource(int amount)
    {
        quantity -= amount;
        int amountToGive = amount;

        if (quantity < 0)
        {
            amountToGive = amount + quantity;
        }

        Hive.instance.GainResource(type, amountToGive);

        // Destroy the resource source if depleted.
        if (quantity <= 0)
        {
            Destroy(gameObject);
        }

        // Trigger the change event if it's set.
        if (onQuantityChange != null)
        {
            onQuantityChange.Invoke();
        }
    }
}
