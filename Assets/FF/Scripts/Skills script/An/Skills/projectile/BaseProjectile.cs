using UnityEngine;
public abstract class BaseProjectile : MonoBehaviour, IElementalObject
{
    public bool usePhisics = false;
    protected ProjectileData data;
    [SerializeField] protected ProjectileEffectBase effect;
    protected Element currentElement;
    protected bool hasReacted = false;
    protected float damage;
    protected float speed = 5f;
    [SerializeField]protected Rigidbody2D rb;
    protected Transform target;
    protected Vector2 targetPosition;

    public Element CurrentElement => currentElement;
    public GameObject GameObject => gameObject;
    public bool CanTriggerReaction => data.canTriggerReaction && !hasReacted;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    public virtual void Initialize(ProjectileData projectileData, Vector2 target, Element element)
    {
        targetPosition = target;
        data = projectileData;
        speed = data.speed;
        currentElement = element;
        damage = data.damage;
        if(usePhisics)Move();
  
        HandleInitialEffects();
        Destroy(gameObject, data.range);
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
        if (CanTriggerReaction)
        {
            if(other.TryGetComponent<ReactionItem>(out var item))
            {
                item.StartReaction(currentElement, gameObject, transform.position);
            }
        }
        OnHit(other);
    }
    protected virtual void Move() 
    {
        if(PauseManager.IsPaused) return;
    }
    protected abstract void OnHit(Collider2D other);
    protected abstract void OnProjectileReachedTarget();

    public virtual void OnReact(ElementalReaction reaction = null, Vector3 position = default)
    {
        Destroy(gameObject);
    }

    protected virtual void FixedUpdate()
    {
        if(PauseManager.IsPaused) return;
        if(usePhisics) Move();
    }
}