using UnityEngine;
using System.Collections;
public class An_M1 : TargetedSkill
{
    [SerializeField] private ProjectileSkillData skillData;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Element_use elementController; // Посилання на контролер елементів

    protected override void UseSkillAtPosition(Vector2 targetPosition)
    {
        if (!CanUseSkill()) return;

        if (skillData.pattern.isBurst)
        {
            StartCoroutine(ShootBurst(targetPosition));
        }
        else
        {
            ShootProjectiles(targetPosition);
        }
    }

    private void ShootProjectiles(Vector2 targetPosition)
    {
        Vector2 direction = ((Vector3)targetPosition - transform.position).normalized;
        float startAngle = skillData.pattern.projectilesCount > 1
            ? -skillData.pattern.angleBetweenProjectiles * (skillData.pattern.projectilesCount - 1) / 2
            : 0;

        for (int i = 0; i < skillData.pattern.projectilesCount; i++)
        {
            float currentAngle = startAngle + skillData.pattern.angleBetweenProjectiles * i;
            Vector2 projectileDirection = RotateVector(direction, currentAngle);

            if (skillData.pattern.hasSpread)
            {
                float spread = Random.Range(-skillData.pattern.spreadAngle, skillData.pattern.spreadAngle);
                projectileDirection = RotateVector(projectileDirection, spread);
            }

            SpawnProjectile(projectileDirection);
        }
    }

    private void SpawnProjectile(Vector2 direction)
    {
        Vector3 spawnPosition = shootPoint != null ? shootPoint.position : transform.position;
        Vector2 targetPosition = (Vector2)spawnPosition + direction * skillData.projectileData.range;

        // Отримуємо поточний елемент
        Element currentElement = elementController != null ? elementController.currentElement : Element.None;

        // Отримуємо відповідний префаб на основі елементу
        GameObject projectilePrefab = skillData.projectileData.GetProjectilePrefab(currentElement);

        GameObject projectileObj = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

        if (projectileObj.TryGetComponent<EnhancedProjectile>(out var projectile))
        {
            projectile.Initialize(skillData.projectileData, targetLayer, shootPoint, targetPosition);
            // Додатково можна передати поточний елемент в projectile
            if (projectile is IElemental elementalProjectile)
            {
                elementalProjectile.SetElement(currentElement);
            }
        }
    }

    private Vector2 RotateVector(Vector2 direction, float angleInDegrees)
    {
        float angleInRadians = angleInDegrees * Mathf.Deg2Rad;
        float cosAngle = Mathf.Cos(angleInRadians);
        float sinAngle = Mathf.Sin(angleInRadians);
        return new Vector2(
            direction.x * cosAngle - direction.y * sinAngle,
            direction.x * sinAngle + direction.y * cosAngle
        );
    }

    private IEnumerator ShootBurst(Vector2 targetPosition)
    {
        for (int i = 0; i < skillData.pattern.burstCount; i++)
        {
            ShootProjectiles(targetPosition);
            yield return new WaitForSeconds(skillData.pattern.burstDelay);
        }
    }
}

// Інтерфейс для елементальних об'єктів
public interface IElemental
{
    void SetElement(Element element);
    Element GetElement();
}