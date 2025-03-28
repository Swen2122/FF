using UnityEngine;
public class MeleeAttackAction : AttackAction
{
    private Animator animator;

    public MeleeAttackAction(BaseEnemyAI enemyAI, Animator anim, float cooldown)
        : base(enemyAI, cooldown)
    {
        animator = anim;
    }

    protected override void PerformAttack()
    {
        enemy.canMove = false;
        animator.SetTrigger("Attack");
    }
}
