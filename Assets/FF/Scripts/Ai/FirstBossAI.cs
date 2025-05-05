using UnityEngine;

public class FirstBossAI : BaseEnemyAI
{
    protected override void SetupBehaviorTree()
    {
        rootNode = new Selector();
        SequenceNode attackSequence = new SequenceNode();
        attackSequence.Attach(new CheckDistanceToPlayer(this, attackRange, true));
        attackSequence.Attach(new UseSkillPosition(this, Controler.Instance.transform.position, TriggerType.damage));
        rootNode.Attach(attackSequence);
        
    }
}
