using UnityEngine;
using DG.Tweening;

public class Projectile2 : MonoBehaviour
{
    // ��������� ������� �������
    private Vector3 startPosition;
    // ֳ����� ������� �������
    private Vector3 targetPosition;
    // ��� ��� �����/�������
    private SkillData skillData;
    // ��������� ���������� ����
    private bool hasCollided = false;

    // ����������� ������� � ���������� �� ������� ���������
    public void Initialize(Vector3 start, Vector3 target, SkillData data)
    {
        startPosition = start;
        targetPosition = target;
        skillData = data;
        Move();
    }

    // ����� ���� �������
    private void Move()
    {
        // ���������� ������ �� �������
        float distance = Vector3.Distance(startPosition, targetPosition);
        // ���������� ��������� ����
        float duration = distance / skillData.projectileSpeed;

        // ���������� ���������� �������
        Vector3[] path = CalculateParabolicPath(duration);

        // ������� ���� � ����������� ��������
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(skillData.movementEase));


        // Destroy the projectile after it reaches the target (or a bit after, adjust as needed)
        Destroy(gameObject, duration + 0.1f);  // Prevents projectiles lingering indefinitely
    }

    // ���������� ���������� ������� ����
    private Vector3[] CalculateParabolicPath(float duration)
    {
        // ���������� �������� ����
        Vector3 direction = (targetPosition - startPosition).normalized;
        // ���������� ������ �� �������
        float distance = Vector3.Distance(startPosition, targetPosition);

        // �������� ������������ ������ �������� ������� �� ������
        float heightMultiplier = Mathf.Clamp(distance / 10f, 0.5f, 2f);
        float maxHeight = skillData.parabolaHeight * heightMultiplier;

        // ���������� �������� ����� ����
        Vector3 midPoint1 = Vector3.Lerp(startPosition, targetPosition, 0.33f);
        Vector3 midPoint2 = Vector3.Lerp(startPosition, targetPosition, 0.66f);

        // ���������� ��������������� ����
        midPoint1.x = Mathf.Lerp(startPosition.x, targetPosition.x, 0.33f);
        midPoint2.x = Mathf.Lerp(startPosition.x, targetPosition.x, 0.66f);

        // ϳ������ �������� �����
        midPoint1.y += maxHeight * 0.7f;
        midPoint2.y += maxHeight * 0.3f;

        // ���������� ������ ����� �������
        return new Vector3[]
        {
            startPosition,
            midPoint1,
            midPoint2,
            targetPosition
        };
    }

    // ����� ������� �������� � 2D �������
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ��������, �� �������� �������� � �������� �� ���� � ��� �� �� ������ �� �� ��������
        if (collision.gameObject.layer == gameObject.layer && !hasCollided)
        {
            // ��������� ������ ������� �� ����������
            hasCollided = true;
            Projectile2 otherProjectile = collision.gameObject.GetComponent<Projectile2>();
            if (otherProjectile != null)
            {
                otherProjectile.hasCollided = true;
            }

            // ��������� ������ ������������ ���� ���� ���
            if (skillData.impactPrefab != null)
            {
                Instantiate(skillData.impactPrefab, collision.contacts[0].point, Quaternion.identity);
            }

            // �������� ���� �������
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}