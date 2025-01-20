using UnityEngine;
using System.Collections;

public class An_M1 : TargetedSkill
{
    [SerializeField] private ProjectileSkillData skillData;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Element_use elementController;
    [SerializeField] private ElementalReactionDatabase reactionDatabase;

    protected override void UseSkillAtPosition(Vector2 targetPosition)
    {
        if (!CanUseSkill()) return;

        if (skillData.pattern.isBurst)
        {
            StartCoroutine(ShootBurst(targetPosition));
        }
        else if (skillData.pattern.useConvergence)
        {
            ShootConvergingProjectiles(targetPosition);
        }
        else
        {
            ShootProjectiles(targetPosition);
        }
    }

    private void ShootProjectiles(Vector2 targetPosition)
    {
        Vector2 direction = ((Vector3)targetPosition - transform.position).normalized;
        float startAngle = CalculateStartAngle();

        for (int i = 0; i < skillData.pattern.projectilesCount; i++)
        {
            float currentAngle = startAngle + skillData.pattern.angleBetweenProjectiles * i;
            Vector2 projectileDirection = RotateVector(direction, currentAngle);

            if (skillData.pattern.hasSpread)
            {
                float spread = Random.Range(-skillData.pattern.spreadAngle, skillData.pattern.spreadAngle);
                projectileDirection = RotateVector(projectileDirection, spread);
            }

            Vector2 finalPosition = (Vector2)transform.position + projectileDirection * skillData.projectileData.range;
            SpawnProjectile(finalPosition);
        }
    }
    private void ShootConvergingProjectiles(Vector2 targetPosition)
    {
        Vector2 direction = ((Vector3)targetPosition - transform.position).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);
        float offset = skillData.pattern.convergenceOffset;
        float curveHeight = skillData.pattern.curveHeight;

        // Спавн снарядів з обох боків
        for (int i = -1; i <= 1; i += 2)
        {
            Vector2 spawnPoint = (Vector2)shootPoint.position + perpendicular * offset * i;

            GameObject prefab = skillData.GetProjectileData(elementController?.currentElement ?? Element.None);
            GameObject projectileObj = Instantiate(prefab, spawnPoint, shootPoint.rotation);

            if (projectileObj.TryGetComponent(out BaseProjectile baseProjectile))
            {
                baseProjectile.Initialize(skillData.projectileData, targetPosition, elementController?.currentElement ?? Element.None);

                if (baseProjectile.trajectoryHandler != null)
                {
                    // Висота кривої залежить від сторони запуску
                    float adjustedHeight = curveHeight * i;
                    baseProjectile.trajectoryHandler.MoveCurve(targetPosition, adjustedHeight, null);
                }
            }
        }
    }
    private void SpawnProjectile(Vector2 targetPosition, bool isConverging = false, int direction = 1, Vector2? spawnPosition = null)
    {
        Vector3 actualSpawnPosition = spawnPosition ?? shootPoint.position;
        Element currentElement = elementController?.currentElement ?? Element.None;
        GameObject prefab = skillData.GetProjectileData(currentElement);
        GameObject projectileObj = Instantiate(prefab, actualSpawnPosition, shootPoint.rotation);

        if (projectileObj.TryGetComponent(out BaseProjectile baseProjectile))
        {
            baseProjectile.Initialize(skillData.projectileData, targetPosition, currentElement);
        }
        else if (projectileObj.TryGetComponent(out EnhancedProjectile enhancedProjectile))
        {
            enhancedProjectile.Initialize(skillData.projectileData, targetPosition, currentElement);
        }
    }

    private float CalculateStartAngle()
    {
        return skillData.pattern.projectilesCount > 1
            ? -skillData.pattern.angleBetweenProjectiles * (skillData.pattern.projectilesCount - 1) / 2
            : 0;
    }

    private Vector2 RotateVector(Vector2 vector, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
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
