using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum RoomState
{
    Blueprint,
    UnderConstruction,
    Built
}

public class Room : MonoBehaviour
{
    public RoomPreset preset;
    public BuildProgressBar progressBar;
    public GameObject doneIcon;

    public int buildProgress;

    public bool concructionDone;

    private void Start()
    {
        concructionDone = false;
        doneIcon.SetActive(false);
    }

    public void BuildRoom(int amount)
    {
        if (buildProgress >= 100)
        {
            concructionDone = true;
            progressBar.CloseProgressBar();
            // deleteRoomButton.SetActive(false);  
            doneIcon.SetActive(true);
            return;
        }

        buildProgress += amount;
        progressBar.UpdateProgressBar(buildProgress, 100);

    }

    public void DeleteRoom()
    {
        Debug.Log("1");
        Hive.instance.OnRemoveBuilding(this);
        
    }

}
