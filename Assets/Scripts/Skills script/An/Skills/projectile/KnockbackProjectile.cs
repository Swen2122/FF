using UnityEngine;
public class KnockbackProjectile : BaseProjectile
{
    [SerializeField] private float pushForce = 10f;
    [SerializeField] private LayerMask pushableLayer;

    protected override void OnHit(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & pushableLayer) != 0)
        {
            if (other.TryGetComponent<Rigidbody2D>(out var rb))
            {
                Vector2 pushDirection = transform.position - other.transform.position;
                rb.AddForce(-pushDirection.normalized * pushForce, ForceMode2D.Impulse);
            }
            if (other.TryGetComponent<ICanHit>(out var target))
            {
                target.TakeHit(damage);
            }
        }
    }

    protected override void OnProjectileReachedTarget()
    {
        Destroy(gameObject);
    }
}