using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIComponents : MonoBehaviour
{
    public static UIComponents instance;

    public GameObject gameOverReminder;
    //private bool isGameOverReminderActive;

    void Awake()
    {
        instance = this;

        gameOverReminder.SetActive(false);
        //isGameOverReminderActive = false;
    }

    /// <summary>
    /// Game over reminder - red blinking page
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

    public void StopGameOverReminder()
    {
        //isGameOverReminderActive = false;
        gameOverReminder.SetActive(false);
    }
}
