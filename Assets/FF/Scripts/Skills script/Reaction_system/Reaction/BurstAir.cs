using UnityEngine;

public class BurstAir : AbstractReactionEffect
{
    public static BurstAir Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public override void Initialize(ElementalReaction.ReactionEffect effect)
    {
        base.Initialize(effect);
        BurstParticleEffectManager.Instance.PlayEffect(transform.position, settings.color, 0.3f);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius, settings.targetLeyer);
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent<ICanHit>(out var damageable))
            {
                damageable.TakeHit(damage, settings.element);
            }
        }
        // Додаткові налаштування для BurstAir, якщо потрібно
    }
    protected override void OnEnergyTick()
    {

    }
    protected override void OnReactionEnd()
    {
       
    }
    protected override void OnReactionDisrupted()
    {

    }
}