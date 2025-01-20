using UnityEngine;

// Дія атаки
public class AttackAction : EnemyBehaviorNode
{
    protected float attackCooldown;
    protected float lastAttackTime;

    public AttackAction(BaseEnemyAI enemyAI, float cooldown = 1f) : base(enemyAI)
    {
        attackCooldown = cooldown;
    }

    public override NodeState Evaluate()
    {
        if (Time.time - lastAttackTime >= attackCooldown)
        {
            PerformAttack();
            lastAttackTime = Time.time;
            state = NodeState.Success;
        }
        else
        {
            state = NodeState.Running;
        }

        return state;
    }

    protected virtual void PerformAttack()
    {
        // Перевизначається в конкретних класах атак
    }
}