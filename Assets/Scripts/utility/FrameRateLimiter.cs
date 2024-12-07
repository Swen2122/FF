using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 60; // ֳ����� ������� �����
    [SerializeField] private bool vSync = false; // ֳ����� ������� �����

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
}