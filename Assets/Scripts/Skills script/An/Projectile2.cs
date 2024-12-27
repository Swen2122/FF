using UnityEngine;
using DG.Tweening;

public class Projectile2 : MonoBehaviour
{
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private SkillData skillData;
    private bool hasCollided = false;
    private int mirrorDirection; // 1 або -1 для дзеркального ефекту
    private bool isHorizontalDominant;

    public void Initialize(Vector3 start, Vector3 target, SkillData data, int mirrorDir, bool horizontalDominant)
    {
        startPosition = start;
        targetPosition = target;
        skillData = data;
        mirrorDirection = mirrorDir;
        isHorizontalDominant = horizontalDominant;
        Move();
    }

    private void Move()
    {
        float distance = Vector3.Distance(startPosition, targetPosition);
        float duration = distance / skillData.projectileSpeed;

        Vector3[] path = CalculateParabolicPath(duration);

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(skillData.movementEase));

        Destroy(gameObject, duration + 0.1f);
    }

    private Vector3[] CalculateParabolicPath(float duration)
    {
        Vector3 direction = (targetPosition - startPosition).normalized;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float heightMultiplier = Mathf.Clamp(distance / 10f, 0.5f, 2f);
        float maxHeight = skillData.parabolaHeight * heightMultiplier;

        // Розраховуємо чотири контрольні точки для кривої
        Vector3 p1 = startPosition;
        Vector3 p4 = targetPosition;

        // Розраховуємо проміжні точки з урахуванням дзеркального ефекту
        float t1 = 0.33f;
        float t2 = 0.66f;

        Vector3 p2 = Vector3.Lerp(startPosition, targetPosition, t1);
        Vector3 p3 = Vector3.Lerp(startPosition, targetPosition, t2);

        // Застосовуємо дзеркальне зміщення залежно від домінуючого напрямку
        if (isHorizontalDominant)
        {
            // Для горизонтального руху зміщуємо по Y
            p2.y += maxHeight * mirrorDirection;
            p3.y += maxHeight * mirrorDirection * 0.5f;
        }
        else
        {
            // Для вертикального руху зміщуємо по X
            p2.x += maxHeight * mirrorDirection;
            p3.x += maxHeight * mirrorDirection * 0.5f;
        }

        return new Vector3[] { p1, p2, p3, p4 };
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == gameObject.layer && !hasCollided)
        {
            hasCollided = true;
            Projectile2 otherProjectile = collision.gameObject.GetComponent<Projectile2>();
            if (otherProjectile != null)
            {
                otherProjectile.hasCollided = true;
            }

            if (skillData.impactPrefab != null)
            {
                Instantiate(skillData.impactPrefab, collision.contacts[0].point, Quaternion.identity);
            }

            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}