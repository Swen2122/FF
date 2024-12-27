using UnityEngine;

public class ConvergenceSkill : TargetedSkill, ISkill
{
    [Header("Skill Configuration")]
    [SerializeField] private ProjectileSkillData skillData;
    [SerializeField] private ConvergenceSkillData convergenceData;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Element_use elementController;
    [SerializeField] private float projectileOffset = 2f;

    protected override void UseSkillAtPosition(Vector2 targetPosition)
    {
        if (!CanUseSkill()) return;

        // Визначаємо початкову позицію та напрямок
        Vector2 startPos = shootPoint != null ? (Vector2)shootPoint.position : (Vector2)transform.position;
        Vector2 direction = (targetPosition - startPos).normalized;

        // Обчислюємо перпендикулярний вектор для зміщення точок спавну
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);

        // Створюємо точки спавну з обох боків
        Vector2 spawnPoint1 = startPos + perpendicular * projectileOffset;
        Vector2 spawnPoint2 = startPos - perpendicular * projectileOffset;

        // Спавнимо снаряди
        SpawnProjectile(spawnPoint1, targetPosition, true);  // Горизонтально домінантний
        SpawnProjectile(spawnPoint2, targetPosition, false); // Не горизонтально домінантний

        lastUseTime = Time.time;
    }

    private void SpawnProjectile(Vector2 spawnPos, Vector2 targetPos, bool isHorizontalDominant)
    {
        // Отримуємо поточний елемент
        Element currentElement = elementController.currentElement;

        // Отримуємо відповідний префаб снаряду
        GameObject projectilePrefab = skillData.projectileData.GetProjectilePrefab(currentElement);

        // Створюємо снаряд
        GameObject projectileObj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        // Ініціалізуємо компонент снаряду
        if (projectileObj.TryGetComponent<ConvergenceProjectile>(out var projectile))
        {
            projectile.Initialize(
                skillData.projectileData,
                convergenceData,
                targetLayer,
                shootPoint,
                targetPos,
                1, // Множник швидкості
                isHorizontalDominant,
                currentElement
            );
        }
    }

    // Реалізація інтерфейсу ISkill
    public void Execute(Vector2 targetPosition) => UseSkillAtPosition(targetPosition);

    public Element GetSkillElement() => elementController.currentElement;
}