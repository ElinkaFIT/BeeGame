using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponents : MonoBehaviour
{
    public static UIComponents instance;

    public GameObject gameOverReminder;
    private bool isGameOverReminderActive;

    void Awake()
    {
        instance = this;

        gameOverReminder.SetActive(false);
        isGameOverReminderActive = false;
    }

    /// <summary>
    /// Game over reminder - red blinking page
    /// </summary>
    public void StartGameOverReminder()
    {
        isGameOverReminderActive = true;
        float lastConsumptionTime = 0;
        bool redIsOn = false;

        while (isGameOverReminderActive)
        {
            if (Time.time - lastConsumptionTime > 1)
            {
                lastConsumptionTime = Time.time;
                gameOverReminder.SetActive(!redIsOn);
                redIsOn = !redIsOn;
            }
        }

    }

    public void StopGameOverReminder()
    {
        isGameOverReminderActive = false;

    }
}
