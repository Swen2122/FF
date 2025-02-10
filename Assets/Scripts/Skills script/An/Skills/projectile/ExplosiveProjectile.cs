using UnityEngine;

public class ExplosiveProjectile : DefaultProjectile
{
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private LayerMask explosionLayer;
    [SerializeField] private GameObject explosionEffectPrefab;

    protected override void OnHit(Collider2D other)
    {
        Explode();
        Destroy(gameObject);
    }

    private void Explode()
    {
        // ����
        Collider2D[] damageHits = Physics2D.OverlapCircleAll(transform.position, explosionRadius, damageLayer);
        foreach (var hit in damageHits)
        {
            if (hit.TryGetComponent<ICanHit>(out var target))
            {
                target.TakeHit(damage, Element.Fire);
            }
        }

        // ������ ������
        if (explosionEffectPrefab)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    protected override void OnProjectileReachedTarget()
    {
        Explode();
        Destroy(gameObject);
    }
}
