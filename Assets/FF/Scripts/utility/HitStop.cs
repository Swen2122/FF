using UnityEngine;
using System.Collections;

public static class HitStop
{
    private static bool isStopping = false;

    /// <summary>
    /// Викликає мікро паузу на заданий час.
    /// </summary>
    /// <param name="duration">Тривалість паузи у секундах.</param>
    /// <param name="timeScaleDuringStop">TimeScale під час паузи (наприклад, 0 для повної зупинки).</param>
    public static void TriggerStop(float duration, float timeScaleDuringStop = 0.0f)
    {
        if (!isStopping)
        {
            GameObject hook = new GameObject("HitStopHook");
            hook.AddComponent<HitStopCoroutine>().Initialize(duration, timeScaleDuringStop);
        }
    }

    private class HitStopCoroutine : MonoBehaviour
    {
        private float duration;
        private float timeScaleDuringStop;

        public void Initialize(float duration, float timeScaleDuringStop)
        {
            this.duration = duration;
            this.timeScaleDuringStop = timeScaleDuringStop;
            StartCoroutine(StopCoroutine());
        }

        private IEnumerator StopCoroutine()
        {
            isStopping = true;

            // Зберігаємо поточний TimeScale
            float originalTimeScale = Time.timeScale;

            // Зупиняємо час
            Time.timeScale = timeScaleDuringStop;

            // Чекаємо тривалість паузи
            yield return new WaitForSecondsRealtime(duration);

            // Відновлюємо TimeScale
            Time.timeScale = originalTimeScale;

            isStopping = false;

            // Знищуємо цей об'єкт
            Destroy(gameObject);
        }
    }
}
