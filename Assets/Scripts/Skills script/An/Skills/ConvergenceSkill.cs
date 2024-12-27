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

        // ��������� ��������� ������� �� ��������
        Vector2 startPos = shootPoint != null ? (Vector2)shootPoint.position : (Vector2)transform.position;
        Vector2 direction = (targetPosition - startPos).normalized;

        // ���������� ���������������� ������ ��� ������� ����� ������
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);

        // ��������� ����� ������ � ���� ����
        Vector2 spawnPoint1 = startPos + perpendicular * projectileOffset;
        Vector2 spawnPoint2 = startPos - perpendicular * projectileOffset;

        // �������� �������
        SpawnProjectile(spawnPoint1, targetPosition, true);  // ������������� ����������
        SpawnProjectile(spawnPoint2, targetPosition, false); // �� ������������� ����������

        lastUseTime = Time.time;
    }

    private void SpawnProjectile(Vector2 spawnPos, Vector2 targetPos, bool isHorizontalDominant)
    {
        // �������� �������� �������
        Element currentElement = elementController.currentElement;

        // �������� ��������� ������ �������
        GameObject projectilePrefab = skillData.projectileData.GetProjectilePrefab(currentElement);

        // ��������� ������
        GameObject projectileObj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        // ���������� ��������� �������
        if (projectileObj.TryGetComponent<ConvergenceProjectile>(out var projectile))
        {
            projectile.Initialize(
                skillData.projectileData,
                convergenceData,
                targetLayer,
                shootPoint,
                targetPos,
                1, // ������� ��������
                isHorizontalDominant,
                currentElement
            );
        }
    }

    // ��������� ���������� ISkill
    public void Execute(Vector2 targetPosition) => UseSkillAtPosition(targetPosition);

    public Element GetSkillElement() => elementController.currentElement;
}