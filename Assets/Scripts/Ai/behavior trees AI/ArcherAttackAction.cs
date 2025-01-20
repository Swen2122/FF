using UnityEngine;
// Приклад конкретної атаки для лучника
public class ArcherAttackAction : AttackAction
{
    private Archer_Skill archerSkill;

    public ArcherAttackAction(BaseEnemyAI enemyAI, Archer_Skill skill, float cooldown = 3f)
        : base(enemyAI, cooldown)
    {
        archerSkill = skill;
    }

    protected override void PerformAttack()
    {
        archerSkill.BoltShot(player, 30f, 30);
    }
}