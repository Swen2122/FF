using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class VisionConfig
{
    public float viewDistance = 5f;     // �� ������ ������ �����
    public float viewAngle = 90f;       // ��� ������ (� ��������)
    public Color visionColor = Color.yellow;  // ���� ��� ����������� � ��������
    public bool drawGizmos = true;      // �� ���������� �������� �������������
}

public class EnemyVision : MonoBehaviour
{
    [Header("")]
    public Transform eyePoint;           
    public VisionConfig visionConfig;   

    [Header(" ")]
    public LayerMask targetMask;       
    public LayerMask obstacleMask;     

    [Header("")]
    public bool useTargetMemory = false;  
    public float memoryDuration = 3f;     

    private Vector3 lastKnownTargetPosition;
    private float lastTimeTargetSeen;
    private bool hasLastKnownPosition;

    private void Start()
    {
        if (eyePoint == null)
        {
            eyePoint = transform;
            Debug.LogWarning("EyePoint");
        }

        if (visionConfig == null)
        {
            visionConfig = new VisionConfig();
        }
    }
    public Transform FindNearestTarget()
    {
        Transform nearestTarget = null;
        float nearestDistance = float.MaxValue;

        Collider2D[] possibleTargets = Physics2D.OverlapCircleAll(
            eyePoint.position,
            visionConfig.viewDistance,
            targetMask
        );

        foreach (Collider2D targetCollider in possibleTargets)
        {
            float distance = Vector2.Distance(eyePoint.position, targetCollider.transform.position);

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

    public bool CanSeeTarget(Transform target)
    {
        if (target == null) return false;

        Vector2 directionToTarget = (target.position - eyePoint.position).normalized;
        float distanceToTarget = Vector2.Distance(eyePoint.position, target.position);

        float angleToTarget = Vector2.Angle(eyePoint.right, directionToTarget);
        if (angleToTarget > visionConfig.viewAngle / 2)
        {
            return false;
        }

        RaycastHit2D hit = Physics2D.Raycast(
            eyePoint.position,
            directionToTarget,
            distanceToTarget,
            obstacleMask
        );

        return hit.collider == null;
    }

    private void UpdateTargetMemory(Vector3 targetPosition)
    {
        lastKnownTargetPosition = targetPosition;
        lastTimeTargetSeen = Time.time;
        hasLastKnownPosition = true;
    }

    public Vector3? GetLastKnownTargetPosition()
    {
        if (!useTargetMemory || !hasLastKnownPosition) return null;

        if (Time.time - lastTimeTargetSeen > memoryDuration)
        {
            hasLastKnownPosition = false;
            return null;
        }

        return lastKnownTargetPosition;
    }
    private void OnDrawGizmos()
    {
        if (!visionConfig.drawGizmos || eyePoint == null) return;

        Gizmos.color = visionConfig.visionColor;
        Gizmos.DrawWireSphere(eyePoint.position, visionConfig.viewDistance);

        Vector3 leftBoundary = Quaternion.Euler(0, 0, -visionConfig.viewAngle / 2) *
                              eyePoint.right * visionConfig.viewDistance;
        Vector3 rightBoundary = Quaternion.Euler(0, 0, visionConfig.viewAngle / 2) *
                               eyePoint.right * visionConfig.viewDistance;

        Gizmos.DrawLine(eyePoint.position, eyePoint.position + leftBoundary);
        Gizmos.DrawLine(eyePoint.position, eyePoint.position + rightBoundary);

        if (useTargetMemory && hasLastKnownPosition)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(lastKnownTargetPosition, 0.5f);
        }
    }
}