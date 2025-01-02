using UnityEngine;
public abstract class BaseProjectile : MonoBehaviour, IElementalObject
{
    protected ProjectileData data;
    protected TrajectoryHandler trajectoryHandler;
    protected Element currentElement;
    protected bool hasReacted = false;
    protected float damage;

    public Element CurrentElement => currentElement;
    public GameObject GameObject => gameObject;

    public virtual void Initialize(ProjectileData projectileData, Vector2 target, Element element)
    {
        data = projectileData;
        currentElement = element;
        damage = data.damage;
        trajectoryHandler = new TrajectoryHandler(transform, data);
        HandleInitialEffects();
        trajectoryHandler.MoveLinear(target, OnProjectileReachedTarget);
    }

    protected void HandleInitialEffects()
    {
        if (data.spawnEffect != null)
            Instantiate(data.spawnEffect, transform.position, Quaternion.identity);
        if (data.trailEffect != null)
            Instantiate(data.trailEffect, transform);
        if (data.launchSound != null)
            AudioSource.PlayClipAtPoint(data.launchSound, transform.position);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        OnHit(other);
        HandleElementalReaction(other);
    }

    protected abstract void OnHit(Collider2D other);
    protected abstract void OnProjectileReachedTarget();

    private void HandleElementalReaction(Collider2D other)
    {
        if (hasReacted) return;
        if (other.TryGetComponent<IElementalObject>(out var otherElemental))
        {
            if (other.GetComponent<BaseProjectile>()?.hasReacted == true) return;
            var handler = FindObjectOfType<ElementalReactionHandler>();
            if (handler != null)
            {
                handler.TriggerReaction(currentElement, otherElemental.CurrentElement, otherElemental.GameObject, transform.position);
            }
            hasReacted = true;
        }
    }

    public virtual void OnReact(ElementalReaction reaction, Vector3 position)
    {
        Destroy(gameObject);
    }
}