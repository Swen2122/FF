using UnityEngine;

public abstract class BaseStatusEffect : ScriptableObject
{
    public string effectId;
    public float duration;
    public float tickInterval;
    protected float lastTickTime;
    protected float durationStartTime;
    protected GameObject target;
    public bool isActive = false;
    
    public virtual void SetDuration(float duration)
    {
        this.duration = duration;
    }
    public virtual void ApplyEffect(GameObject target)
    {
        this.target = target;
        isActive = true;
        durationStartTime = 0f;  // Скидаємо накопичений час
        lastTickTime = 0f;
    }
    public virtual bool UpdateEffect()
    {
        durationStartTime += Time.deltaTime;
        // Оновлюємо час тіку і гарантуємо, що не пропускаємо тики
        while (lastTickTime + tickInterval <= durationStartTime)
        {
            Debug.Log("Effect Tick");
            lastTickTime += tickInterval;
            EffectTick();
        }
        if (durationStartTime >= duration)
        {
            RemoveEffect();
        }
        return isActive;
    }
    protected abstract void EffectTick();
    public virtual void RemoveEffect()
    {
        isActive = false;
    }

}
