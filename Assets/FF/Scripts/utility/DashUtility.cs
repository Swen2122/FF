using UnityEngine;
using DG.Tweening;

public static class DashUtility
{
    // ������ ��� � ��������� ����������� ����� Rigidbody2D.  
    public static void PerformDash(Rigidbody2D target, Vector2 direction, float dashDistance, float dashDuration, LayerMask obstacleLayer, System.Action onComplete = null)
    {
        // ���������� �������� �������� ����� Raycast2D
        RaycastHit2D hit = Physics2D.Raycast(target.position, direction, dashDistance, obstacleLayer);

        // ���� � ���������, ��������� ��������� �� ��
        float moveDistance = hit.collider != null ? hit.distance : dashDistance;

        // ���������� ������ ������� ����
        Vector2 targetPosition = target.position + direction.normalized * moveDistance;

        // ������������� DoTween ��� �������� ���������� � Rigidbody2D
        target.DOMove(targetPosition, dashDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                // ��������� callback ���� ���������� ����
                onComplete?.Invoke();
            });
    }
}