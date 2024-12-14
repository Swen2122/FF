using UnityEngine;
public class ParabolicSkill : BaseSkill
{
    [SerializeField] private SkillData skillData;
    [SerializeField] private Transform shootPoint;

    private void Awake()
    {
        if (shootPoint == null)
        {
            shootPoint = transform;
        }
    }

    public override void Activate(Vector3 targetPosition)
    {
        Vector3 startPosition = shootPoint.position;
        targetPosition = ClampTargetPosition(startPosition, targetPosition, skillData.maxRange);

        // Визначаємо напрямок зміщення на основі орієнтації об'єкта
        Vector3 right = shootPoint.right;
        Vector3 up = shootPoint.up;

        // Визначаємо домінантний напрямок
        float rightMagnitude = Mathf.Abs(right.x) + Mathf.Abs(right.y);
        float upMagnitude = Mathf.Abs(up.x) + Mathf.Abs(up.y);

        if (rightMagnitude > upMagnitude)
        {
            // Пріоритет горизонтального зміщення
            SpawnProjectile(startPosition, targetPosition, -1, right);
            SpawnProjectile(startPosition, targetPosition, 1, right);
        }
        else
        {
            // Пріоритет вертикального зміщення
            SpawnProjectile(startPosition, targetPosition, -1, up);
            SpawnProjectile(startPosition, targetPosition, 1, up);
        }
    }

    private Vector3 ClampTargetPosition(Vector3 startPosition, Vector3 targetPosition, float maxRange)
    {
        Vector3 direction = (targetPosition - startPosition).normalized;
        float distance = Vector3.Distance(startPosition, targetPosition);

        return distance > maxRange
            ? startPosition + direction * maxRange
            : targetPosition;
    }

    private void SpawnProjectile(Vector3 startPosition, Vector3 targetPosition, int direction, Vector3 offsetDirection)
    {
        // Зміщення залежить від напрямку руху та орієнтації об'єкта
        Vector3 offset = offsetDirection * direction * 0.5f;
        Vector3 spawnPosition = startPosition + offset;

        GameObject projectileObject = Instantiate(skillData.projectilePrefab, spawnPosition, shootPoint.rotation);
        Projectile2 projectile = projectileObject.GetComponent<Projectile2>();

        projectile.Initialize(spawnPosition, targetPosition, skillData);
    }
}