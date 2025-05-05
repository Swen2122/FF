using System.Collections;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EnemyAim : MonoBehaviour
{
    [SerializeField] private float lineLength = 5f;        // ������� ����
    [SerializeField] private Vector2 direction = Vector2.right; // �������� ���� (� 2D)
    [SerializeField] private Color lineColor = Color.red;  // ���� ����

    private LineRenderer lineRenderer;

    void Start()
    {
        // ������������ LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // ˳��� ���������� � 2 �����: ������� �� �����

        // ������������ ������� �� ������ ����
        lineRenderer.startColor = lineColor;
        lineRenderer.endColor = lineColor;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;

        // ������������ ������ ��� ������ � 2D
        lineRenderer.useWorldSpace = true; // �������� ����� ������ � ���������� �����������
        UpdateLinePosition();
    }

    void Update()
    {
        if(PauseManager.IsPaused) return;
        // ��������� ������� ����, ���� �� �������� ��� ������� ���������
        UpdateLinePosition();
    }

    void UpdateLinePosition()
    {
        // ��������� ����� - ������� ��'����
        Vector3 startPoint = transform.position;

        // ʳ����� ����� - �� �������� ������� �� ��������
        Vector3 endPoint = startPoint + (Vector3)(direction.normalized * lineLength);

        // ������ ���������� ������� � ���� ����
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }
}
