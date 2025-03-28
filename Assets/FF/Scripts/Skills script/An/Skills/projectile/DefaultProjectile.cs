using UnityEngine;

public class DefaultProjectile : BaseProjectile
{
    public float impactForce = 10f;
    public StatusEffectSO statusEffectDatabase;
    protected override void Move()
    {
       rb.AddForce(transform.up * speed, ForceMode2D.Impulse);
    }

    protected override void OnHit(Collider2D other)
    {
        if (other.TryGetComponent<ICanHit>(out var damageable))
        {
            damageable.TakeHit(damage, currentElement);
        }
        other.gameObject.TryGetComponent<Rigidbody2D>(out var enemyBody);
        if (enemyBody != null)
        {
            enemyBody.AddForce(transform.up * impactForce, ForceMode2D.Impulse);
        }
        effect?.OnHit(other);
        Destroy(gameObject);
    }
        protected override void OnProjectileReachedTarget()
    {
        Destroy(gameObject);
    }
}
