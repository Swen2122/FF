using UnityEngine;
using DG.Tweening;

public static class DashUtility
{
    // Виконує даш з вказаними параметрами через Rigidbody2D.  
    public static void PerformDash(Rigidbody2D target, Vector2 direction, float dashDistance, float dashDuration, LayerMask obstacleLayer, System.Action onComplete = null)
    {
        // Перевіряємо наявність перешкод через Raycast2D
        RaycastHit2D hit = Physics2D.Raycast(target.position, direction, dashDistance, obstacleLayer);

        // Якщо є перешкода, скорочуємо дистанцію до неї
        float moveDistance = hit.collider != null ? hit.distance : dashDistance;

        // Обчислюємо кінцеву позицію дашу
        Vector2 targetPosition = target.position + direction.normalized * moveDistance;

        // Використовуємо DoTween для плавного переміщення з Rigidbody2D
        target.DOMove(targetPosition, dashDuration)
            .SetEase(Ease.InOutSine)
            .OnComplete(() =>
            {
                // Викликаємо callback після завершення дашу
                onComplete?.Invoke();
            });
    }
}