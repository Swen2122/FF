using UnityEngine;

// Приклад реалізації для лучника
public class ArcherBehaviorTree : EnemyBehaviorTree
{
    [SerializeField] private float minDistance = 4f;
    [SerializeField] private Archer_Skill archerSkill;

    protected override void SetupBehaviorTree()
    {
        rootNode = new Selector();

        // Послідовність для атаки
        SequenceNode attackSequence = new SequenceNode();
        attackSequence.Attach(new CheckDistanceToPlayer(this, attackRange, true));
        attackSequence.Attach(new Inverter(new CheckDistanceToPlayer(this, minDistance, true)));
        attackSequence.Attach(new ArcherAttackAction(this, archerSkill, attackCooldown));

        // Послідовність для відступу
        SequenceNode retreatSequence = new SequenceNode();
        retreatSequence.Attach(new CheckDistanceToPlayer(this, minDistance, true));
        // Тут можна додати RetreatAction

        // Послідовність для переслідування
        SequenceNode chaseSequence = new SequenceNode();
        chaseSequence.Attach(new CheckDistanceToPlayer(this, maxChaseDistance, true));
        chaseSequence.Attach(new Inverter(new CheckDistanceToPlayer(this, attackRange, true)));
        chaseSequence.Attach(new ChaseAction(this, updatePathInterval));

        rootNode.Attach(attackSequence);
        rootNode.Attach(retreatSequence);
        rootNode.Attach(chaseSequence);
    }
}