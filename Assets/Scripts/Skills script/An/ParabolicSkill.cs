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

        // ��������� ������ �� ���
        Vector3 directionToTarget = (targetPosition - startPosition).normalized;

        // ���������, �� ��� ���� �������������� �� ������������
        bool isHorizontalDominant = Mathf.Abs(directionToTarget.x) > Mathf.Abs(directionToTarget.y);

        // ��������� ��� ������� � ����������� ��������
        SpawnMirroredProjectiles(startPosition, targetPosition, isHorizontalDominant);
    }

    private void SpawnMirroredProjectiles(Vector3 startPosition, Vector3 targetPosition, bool isHorizontalDominant)
    {
        float offset = 0.5f; // ³������ ������� �� ���������� ��

        Vector3 offsetVector;
        if (isHorizontalDominant)
        {
            // ��� ��������������� ����, ������ �� Y
            offsetVector = new Vector3(0, offset, 0);
        }
        else
        {
            // ��� ������������� ����, ������ �� X
            offsetVector = new Vector3(offset, 0, 0);
        }

        // ��������� ������� � ����������� ��������
        SpawnProjectile(startPosition + offsetVector, targetPosition, 1, isHorizontalDominant);
        SpawnProjectile(startPosition - offsetVector, targetPosition, -1, isHorizontalDominant);
    }

    private Vector3 ClampTargetPosition(Vector3 startPosition, Vector3 targetPosition, float maxRange)
    {
        Vector3 direction = (targetPosition - startPosition).normalized;
        float distance = Vector3.Distance(startPosition, targetPosition);

        return distance > maxRange
            ? startPosition + direction * maxRange
            : targetPosition;
    }

    private void SpawnProjectile(Vector3 spawnPosition, Vector3 targetPosition, int mirrorDirection, bool isHorizontalDominant)
    {
        GameObject projectileObject = Instantiate(skillData.projectilePrefab, spawnPosition, shootPoint.rotation);
        Projectile2 projectile = projectileObject.GetComponent<Projectile2>();

        projectile.Initialize(spawnPosition, targetPosition, skillData, mirrorDirection, isHorizontalDominant);
    }
}