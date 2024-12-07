using UnityEngine;
using System.Collections;

public static class HitStop
{
    private static bool isStopping = false;

    /// <summary>
    /// ������� ���� ����� �� ������� ���.
    /// </summary>
    /// <param name="duration">��������� ����� � ��������.</param>
    /// <param name="timeScaleDuringStop">TimeScale �� ��� ����� (���������, 0 ��� ����� �������).</param>
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

            // �������� �������� TimeScale
            float originalTimeScale = Time.timeScale;

            // ��������� ���
            Time.timeScale = timeScaleDuringStop;

            // ������ ��������� �����
            yield return new WaitForSecondsRealtime(duration);

            // ³��������� TimeScale
            Time.timeScale = originalTimeScale;

            isStopping = false;

            // ������� ��� ��'���
            Destroy(gameObject);
        }
    }
}
