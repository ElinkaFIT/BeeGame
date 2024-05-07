//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.Collections.Generic;
using UnityEngine;

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
            doneIcon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
        }
    }

    public void StopWorkInRoom(Unit newWorker)
    {
        // Oddelej vcelu ze seznamu pracovniku
        if (roomWorkers.Contains(newWorker))
        {
            roomWorkers.Remove(newWorker);
            doneIcon.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void DeleteRoom()
    {
        Debug.Log("1");
        Hive.instance.OnRemoveBuilding(this);
        
    }

}
