using System.Collections.Generic;
using UnityEngine;

public class ChainProjectile : BaseProjectile
{
    [SerializeField] private float chainRange = 5f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Element damageType;
    private List<GameObject> hitTargets = new List<GameObject>();

    protected override void OnHit(Collider2D other)
    {
        if (other.TryGetComponent<ICanHit>(out var target))
        {
            target.TakeHit(damage, damageType);
            hitTargets.Add(other.gameObject);
            FindAndChainToNextTarget();
        }
    }

    private void FindAndChainToNextTarget()
    {
        Collider2D[] nearbyTargets = Physics2D.OverlapCircleAll(transform.position, chainRange, targetLayer);

        foreach (var target in nearbyTargets)
        {
            if (!hitTargets.Contains(target.gameObject))
            {
                Vector2 newTarget = target.transform.position;
                trajectoryHandler.MoveLinear(newTarget, OnProjectileReachedTarget);
                return;
            }
        }

        // ���� ����� ����� ����
        OnProjectileReachedTarget();
    }

    protected override void OnProjectileReachedTarget()
    {
        if (hitTargets.Count == 0)
        {
            Destroy(gameObject);
        }
    }
}