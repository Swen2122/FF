using UnityEngine;
public abstract class BaseProjectile : MonoBehaviour, IElementalObject, IReactionTrigger
{
    protected ProjectileData data;
    public TrajectoryHandler trajectoryHandler;
    protected Element currentElement;
    protected bool hasReacted = false;
    protected float damage;

    public Element CurrentElement => currentElement;
    public GameObject GameObject => gameObject;
    public bool CanTriggerReaction => data.canTriggerReaction && !hasReacted;

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

        if (data.canTriggerReaction && !hasReacted)
        {
            if (other.TryGetComponent<IElementalObject>(out var otherElemental))
            {
                var handler = Object.FindAnyObjectByType<ElementalReactionHandler>();
                if (handler != null)
                {
                    handler.TriggerReaction(currentElement, otherElemental.CurrentElement,
                        otherElemental.GameObject, transform.position);
                    hasReacted = true;
                }
            }
        }
    }

    protected abstract void OnHit(Collider2D other);
    protected abstract void OnProjectileReachedTarget();

    public virtual void OnReact(ElementalReaction reaction, Vector3 position)
    {
        Destroy(gameObject);
    }
}