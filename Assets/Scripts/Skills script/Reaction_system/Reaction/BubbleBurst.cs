using UnityEngine;

public class BubbleBurst : AbstractReactionEffect
{
    [SerializeField] protected float burstDamage;
    [SerializeField] protected float burstRadius = 1;
    protected override void OnEnergyTick()
    {
        burstDamage += settings.energyPerTick;
        burstRadius += settings.energyPerTick/30;
        maxEnergy -= settings.energyPerTick;
    }
    protected override void OnReactionEnd()
    {
        Burst(burstRadius, burstDamage);
    }
    protected override void OnReactionDisrupted()
    {
        Burst(burstRadius, burstDamage);
    }
    protected virtual void Burst(float radius, float damage)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, settings.targetLeyer);
        foreach(Collider2D hit in hits)
        {
            if (hit.TryGetComponent<ICanHit>(out ICanHit canHit))
            {
                canHit.TakeHit(burstDamage, Element.Water);
            }
        }
        Destroy(gameObject);
        
    }
}
