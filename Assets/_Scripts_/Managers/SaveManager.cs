//****************************************************************************
// Author:      Alena Klimecka (xklime47)
// Project:     Bachelor thesis - Beetween the flowers
// Date:        09/05/2024
//****************************************************************************
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public class SavedData
    {
        public Hive hive;
        public Player player;
    }

    private void Start()
    {
        instance = this;
    }

    public void SaveGame()
    {
        SavedData data = new SavedData();
        data.hive = Hive.instance;
        data.player = Player.me;

        string path = Application.dataPath + "/save.bee";
        string json = JsonUtility.ToJson(data, true);

        File.WriteAllText(path, json);
        
    }

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
