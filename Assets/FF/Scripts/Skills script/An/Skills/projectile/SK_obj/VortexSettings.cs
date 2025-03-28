using UnityEngine;

// ������������ ������������
[CreateAssetMenu(fileName = "New Vortex Settings", menuName = "Element/Vortex Settings")]
public class VortexSettings : ScriptableObject
{
    [Header("������������ �����������")]
    public float pullForce = 5f;        // ���� �����������
    public float pullRadius = 5f;       // ����� �����������
    public float pullInterval = 0.1f;   // �������� ������������
}
