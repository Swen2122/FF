using UnityEngine;
public class EnhancedProjectile : MonoBehaviour, IElementalObject
{
    private ProjectileData data;
    private TrajectoryHandler trajectoryHandler;
    private Element currentElement;
    private bool hasReacted = false;    // Прапорець для відстеження реакції

    public Element CurrentElement => currentElement;
    public GameObject GameObject => gameObject;

    public void Initialize(ProjectileData projectileData, Vector2 target, Element element)
    {
        data = projectileData;
        currentElement = element;
        trajectoryHandler = new TrajectoryHandler(transform, data);

        HandleInitialEffects();
        trajectoryHandler.MoveLinear(target, OnProjectileReachedTarget);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasReacted) return; // Пропускаємо, якщо вже відреагував

        if (other.TryGetComponent<IElementalObject>(out var otherElemental))
        {
            if (other.GetComponent<EnhancedProjectile>()?.hasReacted == true) return;

            HandleElementalReaction(otherElemental);
            hasReacted = true;
            Destroy(gameObject);
            Destroy(otherElemental.GameObject);
        }
    }

    private void HandleInitialEffects()
    {
        if (data.spawnEffect != null)
            Instantiate(data.spawnEffect, transform.position, Quaternion.identity);

        if (data.trailEffect != null)
            Instantiate(data.trailEffect, transform);

        if (data.launchSound != null)
            AudioSource.PlayClipAtPoint(data.launchSound, transform.position);
    }

    private void OnProjectileReachedTarget()
    {
        HandleImpactEffects();
        Destroy(gameObject);
    }

    private void HandleImpactEffects()
    {
        if (data.hitEffect != null)
            Instantiate(data.hitEffect, transform.position, Quaternion.identity);

        if (data.hitSound != null)
            AudioSource.PlayClipAtPoint(data.hitSound, transform.position);
    }
    private void HandleElementalReaction(IElementalObject other)
    {
        var handler = FindObjectOfType<ElementalReactionHandler>();
        if (handler == null) return;

        handler.TriggerReaction(currentElement, other.CurrentElement, other.GameObject, transform.position);
    }

    public void OnReact(ElementalReaction reaction, Vector3 position)
    {
        Debug.Log($"Reacted with {reaction.FirstElement} + {reaction.SecondElement} at {position}");
        Destroy(gameObject);
    }
}