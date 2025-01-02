using UnityEngine;

public class SlowEffect : BaseStatusEffect
{
    public float slowAmount { get; }
    private EnemyMovement movement;
    private BaseEnemyAI enemy;
    private float originalUpdate;
    private float originalSpeed;
    public SlowEffect(float duration, float slowAmount) : base(duration, "Slow")
    {
        this.slowAmount = slowAmount;
    }
    public override void Apply(GameObject target)
    {
        base.Apply(target);
        movement = target.GetComponent<EnemyMovement>();
        enemy = target.GetComponent<BaseEnemyAI>();
        if (movement != null)
        {
            // Зберігаємо оригінальну швидкість
            originalSpeed = movement.speed;
            movement.speed *= (1 - slowAmount);
        }
        if(enemy != null)
        {
            originalUpdate = enemy.updatePathInterval;
            enemy.updatePathInterval *= (1 - slowAmount);
        }
    }

    public override void Remove()
    {
        if (movement != null)
        {
            // Відновлюємо оригінальну швидкість
            enemy.updatePathInterval = originalUpdate;
            movement.speed = originalSpeed;
        }
    }
}