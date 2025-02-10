using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class VisionConfig
{
    public float viewDistance = 5f;     // Як далеко бачить ворог
    public float viewAngle = 90f;       // Кут огляду (в градусах)
    public Color visionColor = Color.yellow;  // Колір для відображення в редакторі
    public bool drawGizmos = true;      // Чи показувати візуальне представлення
}

public class EnemyVision : MonoBehaviour
{
    [Header("Основні налаштування")]
    public Transform eyePoint;           // Точка, звідки дивиться ворог
    public VisionConfig visionConfig;    // Налаштування поля зору

    [Header("Налаштування шарів")]
    public LayerMask targetMask;        // Шари об'єктів, які ворог може бачити
    public LayerMask obstacleMask;      // Шари перешкод (стіни, тощо)

    [Header("Додаткові налаштування")]
    public bool useTargetMemory = false;  // Чи запам'ятовувати останню позицію цілі
    public float memoryDuration = 3f;     // Як довго пам'ятати ціль

    private Vector3 lastKnownTargetPosition;
    private float lastTimeTargetSeen;
    private bool hasLastKnownPosition;

    private void Start()
    {
        // Перевіряємо, чи всі компоненти налаштовані правильно
        if (eyePoint == null)
        {
            eyePoint = transform;
            Debug.LogWarning("EyePoint не встановлено. Використовується позиція об'єкта.");
        }

        // Ініціалізуємо налаштування, якщо вони пусті
        if (visionConfig == null)
        {
            visionConfig = new VisionConfig();
        }
    }

    /// <summary>
    /// Знаходить найближчу ціль у полі зору
    /// </summary>
    public Transform FindNearestTarget()
    {
        Transform nearestTarget = null;
        float nearestDistance = float.MaxValue;

        // Отримуємо всі цілі в радіусі
        Collider2D[] possibleTargets = Physics2D.OverlapCircleAll(
            eyePoint.position,
            visionConfig.viewDistance,
            targetMask
        );

        foreach (Collider2D targetCollider in possibleTargets)
        {
            float distance = Vector2.Distance(eyePoint.position, targetCollider.transform.position);

            // Перевіряємо, чи ціль ближче за попередню найближчу
            if (distance < nearestDistance && CanSeeTarget(targetCollider.transform))
            {
                nearestTarget = targetCollider.transform;
                nearestDistance = distance;

                if (useTargetMemory)
                {
                    UpdateTargetMemory(targetCollider.transform.position);
                }
            }
        }

        return nearestTarget;
    }

    /// <summary>
    /// Перевіряє, чи видно конкретну ціль
    /// </summary>
    public bool CanSeeTarget(Transform target)
    {
        if (target == null) return false;

        Vector2 directionToTarget = (target.position - eyePoint.position).normalized;
        float distanceToTarget = Vector2.Distance(eyePoint.position, target.position);

        // Перевіряємо, чи ціль в межах кута огляду
        float angleToTarget = Vector2.Angle(eyePoint.right, directionToTarget);
        if (angleToTarget > visionConfig.viewAngle / 2)
        {
            return false;
        }

        // Перевіряємо, чи немає перешкод до цілі
        RaycastHit2D hit = Physics2D.Raycast(
            eyePoint.position,
            directionToTarget,
            distanceToTarget,
            obstacleMask
        );

        return hit.collider == null;
    }

    /// <summary>
    /// Оновлює пам'ять про останню позицію цілі
    /// </summary>
    private void UpdateTargetMemory(Vector3 targetPosition)
    {
        lastKnownTargetPosition = targetPosition;
        lastTimeTargetSeen = Time.time;
        hasLastKnownPosition = true;
    }

    /// <summary>
    /// Повертає останню відому позицію цілі
    /// </summary>
    public Vector3? GetLastKnownTargetPosition()
    {
        if (!useTargetMemory || !hasLastKnownPosition) return null;

        // Перевіряємо, чи не застаріла інформація
        if (Time.time - lastTimeTargetSeen > memoryDuration)
        {
            hasLastKnownPosition = false;
            return null;
        }

        return lastKnownTargetPosition;
    }

    /// <summary>
    /// Малює візуальне представлення поля зору в редакторі Unity
    /// </summary>
    private void OnDrawGizmos()
    {
        if (!visionConfig.drawGizmos || eyePoint == null) return;

        // Малюємо коло радіусу огляду
        Gizmos.color = visionConfig.visionColor;
        Gizmos.DrawWireSphere(eyePoint.position, visionConfig.viewDistance);

        // Малюємо кут огляду
        Vector3 leftBoundary = Quaternion.Euler(0, 0, -visionConfig.viewAngle / 2) *
                              eyePoint.right * visionConfig.viewDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, visionConfig.viewAngle / 2) *
                               eyePoint.right * visionConfig.viewDistance;

        Gizmos.DrawLine(eyePoint.position, eyePoint.position + leftBoundary);
        Gizmos.DrawLine(eyePoint.position, eyePoint.position + rightBoundary);

        // Малюємо останню відому позицію цілі
        if (useTargetMemory && hasLastKnownPosition)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(lastKnownTargetPosition, 0.5f);
        }
    }
}