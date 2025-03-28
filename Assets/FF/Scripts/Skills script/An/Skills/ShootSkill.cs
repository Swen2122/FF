using UnityEngine;
using System.Collections;

public class ShootSkill : TargetedSkill
{
    [SerializeField] protected ProjectileSkillData skillData;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected Element_use elementController;
    [SerializeField] protected ElementalReactionDatabase reactionDatabase;

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

    protected void ShootProjectiles(Vector2 targetPosition)
    {
        Vector2 direction = ((Vector3)targetPosition - shootPoint.position).normalized;
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

            Vector2 finalPosition = (Vector2)shootPoint.position + projectileDirection * skillData.projectileData.range;
            SpawnProjectile(finalPosition, projectileDirection);
        }
    }

    protected void SpawnProjectile(Vector2 targetPosition, Vector2 direction)
    {
        Vector3 actualSpawnPosition = shootPoint.position;
        Element currentElement = elementController?.currentElement ?? Element.None;
        GameObject prefab = skillData.GetProjectileData(currentElement);
        GameObject projectileObj = Instantiate(prefab, actualSpawnPosition, Quaternion.LookRotation(Vector3.forward, direction));

        if (projectileObj.TryGetComponent(out BaseProjectile baseProjectile))
        {
            baseProjectile.Initialize(skillData.projectileData, targetPosition, currentElement);
        }
    }

    protected float CalculateStartAngle()
    {
        return skillData.pattern.projectilesCount > 1
            ? -skillData.pattern.angleBetweenProjectiles * (skillData.pattern.projectilesCount - 1) / 2
            : 0;
    }

    protected Vector2 RotateVector(Vector2 vector, float angle)
    {
        float rad = angle * Mathf.Deg2Rad;
        float cos = Mathf.Cos(rad);
        float sin = Mathf.Sin(rad);
        return new Vector2(
            vector.x * cos - vector.y * sin,
            vector.x * sin + vector.y * cos
        );
    }

    protected IEnumerator ShootBurst(Vector2 targetPosition)
    {
        for (int i = 0; i < skillData.pattern.burstCount; i++)
        {
            ShootProjectiles(targetPosition);
            yield return new WaitForSeconds(skillData.pattern.burstDelay);
        }
    }
}