//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.IO;
using UnityEngine;

/// <summary>
/// Manages saving and loading game data.
/// </summary>
public class SaveManager : MonoBehaviour
{
    // Singleton instance of SaveManager.
    public static SaveManager instance; 

    /// <summary>
    /// Represents the data to save or load.
    /// </summary>
    public class SavedData
    {
        public Hive hive;       // Represents the state of the hive.
        public Player player;   // Represents the state of the player.
    }

    /// <summary>
    /// Sets the singleton instance of SaveManager.
    /// </summary>
    private void Start()
    {
        instance = this;
    }

    /// <summary>
    /// Saves the current game state to a file.
    /// </summary>
    public void SaveGame()
    {
        SavedData data = new SavedData();
        data.hive = Hive.instance;
        data.player = Player.me;

        string path = Application.dataPath + "/save.bee";
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    /// <summary>
    /// Loads the game state from a file if it exists.
    /// </summary>
    public void LoadGame()
    {
        string path = Application.dataPath + "/save.bee";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SavedData data = JsonUtility.FromJson<SavedData>(json);

            Hive.instance = data.hive;
            Player.me = data.player;
        }
    }
}
