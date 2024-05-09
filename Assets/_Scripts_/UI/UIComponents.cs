//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using UnityEngine;

/// <summary>
/// Manages UI components related to game state reminders and alerts.
/// </summary>
public class UIComponents : MonoBehaviour
{
    // Singleton instance of UIComponents.
    public static UIComponents instance;

    // GameObject used to display the game over reminder.
    public GameObject gameOverReminder; 
    //private bool isGameOverReminderActive;

    /// <summary>
    /// Initializes the singleton instance and sets the game over reminder to inactive.
    /// </summary>
    void Awake()
    {
        instance = this;
        // Ensure the game over reminder is turned off on start.
        gameOverReminder.SetActive(false); 
        //isGameOverReminderActive = false;
    }

    /// <summary>
    /// Activates the game over reminder UI to alert the player.
    /// </summary>
    public void StartGameOverReminder()
    {
        gameOverReminder.SetActive(true); 

        //isGameOverReminderActive = true;
        //bool redIsOn = false;

        //while (isGameOverReminderActive)
        //{
        //    gameOverReminder.SetActive(!redIsOn);
        //    redIsOn = !redIsOn;

        //    BlinkingWait();
        //}

    }

    //IEnumerator BlinkingWait()
    //{
    //    yield return new WaitForSeconds(3f);
    //}

    /// <summary>
    /// Deactivates the game over reminder UI.
    /// </summary>
    public void StopGameOverReminder()
    {
        //isGameOverReminderActive = false;
        gameOverReminder.SetActive(false);
    }
}
