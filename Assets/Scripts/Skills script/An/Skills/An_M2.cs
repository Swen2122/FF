using System.Collections;
using UnityEngine;

public class An_M2 : ShootSkill
{
    [SerializeField] private int projectilePairsCount = 2;
    [SerializeField] private float arcHeight = 2f;
    [SerializeField] private float spreadAngle = 30f;
    [SerializeField] private float delayBetweenPairs = 0.1f;
    private bool isLeft = false;
    protected override void UseSkillAtPosition(Vector2 targetPosition)
    {
        if (!CanUseSkill()) return;
        StartCoroutine(ShootProjectilePairs(targetPosition));
    }

    private IEnumerator ShootProjectilePairs(Vector2 targetPosition)
    {
        Vector2 direction = ((Vector3)targetPosition - shootPoint.position).normalized;
        Vector2 perpendicular = new Vector2(-direction.y, direction.x);
        float offset = 2;
        float curveHeight = skillData.pattern.curveHeight;

        for (int pairIndex = 0; pairIndex < projectilePairsCount; pairIndex++)
        {
            // Спавн снарядів з обох боків
            for (int i = -1; i <= 1; i += 2)
            {
                Vector2 spawnPoint = (Vector2)shootPoint.position + perpendicular * offset * i;
                GameObject prefab = skillData.GetProjectileData(elementController?.currentElement ?? Element.None);
                GameObject projectileObj = Instantiate(prefab, spawnPoint, shootPoint.rotation);
                int directionMultiplier = isLeft ? i : -i;
                isLeft = !isLeft;
                if (projectileObj.TryGetComponent(out CurvedProjectile curvedProjectile))
                {
                    curvedProjectile.Initialize(
                        skillData.projectileData,
                        targetPosition,
                        elementController.currentElement,
                        curveHeight * directionMultiplier,
                        i
                    );
                }
            }
            yield return new WaitForSeconds(delayBetweenPairs);
        }
    }
}