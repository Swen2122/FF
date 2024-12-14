using UnityEngine;
using UnityEngine.SceneManagement;
public class FrameRateLimiter : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 60; // Цільова частота кадрів
    [SerializeField] private bool vSync = false; 
    [SerializeField] private string menuSceneName = "Menu"; // Назва сцени головного меню


    void Start()
    {
        // Обмежуємо FPS
        Application.targetFrameRate = targetFrameRate;

        // Встановлюємо максимальну частоту оновлення для монітора
        if (vSync)
        {
            QualitySettings.vSyncCount = 1; // 1: синхронізувати з частотою монітора
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        
    }
    public void Exit()
    {
        // Перевіряємо, чи сцена існує
        if (!string.IsNullOrEmpty(menuSceneName))
        {
            // Завантажуємо сцену головного меню
            SceneManager.LoadScene(menuSceneName);
        }
    }
}