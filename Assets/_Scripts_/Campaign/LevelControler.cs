using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelControler : MonoBehaviour
{
    public int nextSceneBuildIndex;
    
    public void NextLevel()
    {
        SceneManager.LoadScene(nextSceneBuildIndex);
    }
}
