using UnityEngine;
// ������� ��������� ����� ��� �������
public class ArcherAttackAction : AttackAction
{
    private ArcherSkill archerSkill;

    public ArcherAttackAction(BaseEnemyAI enemyAI, ArcherSkill skill, float cooldown = 3f)
        : base(enemyAI, cooldown)
    {
        archerSkill = skill;
    }

    protected override void PerformAttack()
    {
        archerSkill.BoltShot(player, 30f, 30);
    }
}