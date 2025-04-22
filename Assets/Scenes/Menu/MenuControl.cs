using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuControl : MonoBehaviour
{
    public void StartPressed()
    {
        LevelLoader.LoadLevel("An-1");
    }

    public void ExitPressed()
    {
        Application.Quit();
        Debug.Log("Exit pressed!");
    }
}
