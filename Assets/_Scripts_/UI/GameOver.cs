using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public void Start()
    {
        gameObject.SetActive(false);
    }
    public void OpenGameOverMenu()
    {
        gameObject.SetActive(true);
    }

    public void Reset()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
