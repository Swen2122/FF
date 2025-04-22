using UnityEngine;
using System;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    public static bool IsPaused { get; private set; }
    public GameObject pauseMenuUI;
    public event Action OnPause;
    public event Action OnResume;
    public KeyCode PauseKey = KeyCode.Escape;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            pauseMenuUI.SetActive(false);
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void Update()
    {
        if (Input.GetKeyDown(PauseKey))
        {
            TogglePause();
        }
    }

    public void Pause()
    {
        if (IsPaused) return;
        pauseMenuUI.SetActive(true);
        IsPaused = true;
        Time.timeScale = 0f;
        OnPause?.Invoke();
    }

    public void Resume()
    {
        if (!IsPaused) return;
        pauseMenuUI.SetActive(false);
        IsPaused = false;
        Time.timeScale = 1f;
        OnResume?.Invoke();
    }

    public void TogglePause()
    {
        if (IsPaused) Resume();
        else Pause();
    }
    public void QuitMenu()
    {
        Resume();
        LevelLoader.LoadLevel("MainMenu");
        Debug.Log("Quitting game...");
    }
}
