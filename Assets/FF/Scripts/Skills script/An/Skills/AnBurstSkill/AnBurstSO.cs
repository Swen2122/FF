using UnityEngine;

public abstract class AnBurstSO : ScriptableObject
{
    public float range;
    public float damage;
    public float duration;
    public float tickInterval;
    public Element element;
    protected float lastTickTime;
    protected float durationStartTime;
    protected bool isActive = true;
    protected Transform ownerTransform;
    public virtual void UseBurst(Transform playerPosition)
    {
        ownerTransform = playerPosition;
        isActive = true;
        durationStartTime = 0f;  // Скидаємо накопичений час
        lastTickTime = 0f;
    }

    public void UpdateBurst() 
    {
        durationStartTime += Time.deltaTime;
        // Оновлюємо час тіку і гарантуємо, що не пропускаємо тики
        while (lastTickTime + tickInterval <= durationStartTime)
        {
            Debug.Log("Tick");
            lastTickTime += tickInterval;
            ApplyEffect();
        }
        if (durationStartTime >= duration)
        {
            Debug.Log("Burst ended");
            isActive = false;
            return;
        }
    }    
    protected abstract void ApplyEffect();
    public bool IsBursting()
    {
        Debug.Log(isActive);
        return isActive;
    } 
}
