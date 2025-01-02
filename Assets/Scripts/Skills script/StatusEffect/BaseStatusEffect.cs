using UnityEngine;
public abstract class BaseStatusEffect : IStatusEffect
{
    protected GameObject target;
    protected float remainingDuration;
    protected float initialDuration;
    protected string effectId;

    public float Duration => initialDuration;
    public bool IsFinished => remainingDuration <= 0;
    public string EffectId => effectId;

    protected BaseStatusEffect(float duration, string effectId)
    {
        this.initialDuration = duration;
        this.remainingDuration = duration;
        this.effectId = effectId;
    }
    public static BaseStatusEffect CreateEffect(BaseStatusEffect template, float duration, float additionalValue = 0)
    {
        switch (template)
        {
            case SlowEffect slowEffect:
                return new SlowEffect(duration, slowEffect.slowAmount);
            case DotEffect dotEffect:
                return new DotEffect(duration, dotEffect.damagePerTick, dotEffect.tickInterval);
            default:
                Debug.LogWarning($"Effect type {template.GetType().Name} is not supported.");
                return null;
        }
    }
    public virtual void Apply(GameObject target)
    {
        this.target = target;
    }

    public virtual void Update()
    {
        remainingDuration -= Time.deltaTime;
    }

    public virtual void Remove()
    {
        // Очистка ефекту
    }
}
