using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public GameObject blueprint;

    public int buildProgress;
    public int buildModifier;

    public bool concructionDone;

    public int curRoomHealth;

    public List<Unit> roomWorkers;

    private void Start()
    {
        curRoomHealth = preset.roomHealthMax;
        concructionDone = false;
        doneIcon.SetActive(false);
    }

    public void BuildRoom(int amount)
    {
        if (buildProgress >= 100)
        {
            concructionDone = true;
            progressBar.CloseProgressBar();
            blueprint.SetActive(false);
            // deleteRoomButton.SetActive(false);  
            doneIcon.SetActive(true);
            return;
        }

        buildProgress = buildProgress + buildModifier + amount;
        progressBar.UpdateProgressBar(buildProgress, 100);

    }

    public void TakeRoomDmg(int amount)
    {
        curRoomHealth -= amount;

        if (curRoomHealth <= 0)
        {
            Hive.instance.rooms.Remove(this);
            Destroy(gameObject);
            return;
        }

    }

    public void WorkInRoom(Unit newWorker)
    {
        // Pridej vcelu do seznamu pracovniku
        if (!roomWorkers.Contains(newWorker)) {
            roomWorkers.Add(newWorker);
        }
    }

    public void StopWorkInRoom(Unit newWorker)
    {
        // Oddelej vcelu ze seznamu pracovniku
        if (roomWorkers.Contains(newWorker))
        {
            roomWorkers.Remove(newWorker);
        }
    }

    public void DeleteRoom()
    {
        Debug.Log("1");
        Hive.instance.OnRemoveBuilding(this);
        
    }

}
