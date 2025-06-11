using System;
using UnityEngine;

public class LevelExit : MonoBehaviour, IInteractible
{
    public string levelName = "MainMenu";
    public void Use()
    {
        LevelLoader.LoadLevel(levelName);
    }
    public void On()
    {
    }
    public void Off()
    {
    }
}
