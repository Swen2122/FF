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
        Time.timeScale = 0f; // Зупинка часу
        Invoke(nameof(ResumeGame), delayDuration); // Виклик через 1/20 секунди
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f; // Відновлення часу
    }
}