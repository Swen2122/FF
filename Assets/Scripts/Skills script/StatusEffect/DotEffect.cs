using UnityEngine;

public class DotEffect : BaseStatusEffect
{
    public float damagePerTick { get; }
    public float tickInterval { get; }
    private float lastTickTime;

    public DotEffect(float duration, float damagePerTick, float tickInterval)
        : base(duration, "DoT")
    {
        this.damagePerTick = damagePerTick;
        this.tickInterval = tickInterval;
        this.lastTickTime = Time.time;
    }

    public override void Update()
    {
        base.Update();
        if (Time.time - lastTickTime >= tickInterval)
        {
            lastTickTime = Time.time;
            // Наносимо шкоду
            var health = target.GetComponent<ICanHit>(); // Припустимо, що є компонент Health
            if (health != null)
            {
                health.TakeHit(damagePerTick);
            }
        }
    }
}

