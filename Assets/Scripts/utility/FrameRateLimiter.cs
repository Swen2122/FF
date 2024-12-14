using UnityEngine;
using UnityEngine.SceneManagement;
public class FrameRateLimiter : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 60; // ֳ����� ������� �����
    [SerializeField] private bool vSync = false; 
    [SerializeField] private string menuSceneName = "Menu"; // ����� ����� ��������� ����


    void Start()
    {
        // �������� FPS
        Application.targetFrameRate = targetFrameRate;

        // ������������ ����������� ������� ��������� ��� �������
        if (vSync)
        {
            QualitySettings.vSyncCount = 1; // 1: ������������� � �������� �������
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
        
    }
    public void Exit()
    {
        // ����������, �� ����� ����
        if (!string.IsNullOrEmpty(menuSceneName))
        {
            // ����������� ����� ��������� ����
            SceneManager.LoadScene(menuSceneName);
        }
    }
}