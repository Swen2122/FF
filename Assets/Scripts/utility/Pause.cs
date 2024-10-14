using System.Collections;
using UnityEngine;

public class Pause : MonoBehaviour
{
  /*  public IEnumerator delay(float delayDuration)
    {
        Time.timeScale = 0f;
        yield return new WaitForSeconds(delayDuration);
        Time.timeScale = 1f;
    } */
    public void delay(float delayDuration)
    {
        Time.timeScale = 0f; // ������� ����
        Invoke(nameof(ResumeGame), delayDuration); // ������ ����� 1/20 �������
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // ³��������� ����
    }
}