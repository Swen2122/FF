using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 60; // Цільова частота кадрів
    [SerializeField] private bool vSync = false; // Цільова частота кадрів

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
}