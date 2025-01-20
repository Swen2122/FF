using UnityEngine;

// Переробка лучника
public class ArcherAI : BaseEnemyAI
{
    [Header("Archer Settings")]
    [SerializeField] private float minDistance = 4f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float shotCooldown = 3f;
    [SerializeField] private Archer_Skill archerSkill;

    protected override void SetupBehaviorTree()
    {
        rootNode = new Selector();

        // Послідовність для атаки
        SequenceNode attackSequence = new SequenceNode();
        attackSequence.Attach(new CheckDistanceToPlayer(this, attackRange, true));
        attackSequence.Attach(new Inverter(new CheckDistanceToPlayer(this, minDistance, true)));
        attackSequence.Attach(new Inverter(new CheckLineOfSight(this, obstacleLayer)));
        attackSequence.Attach(new ArcherAttackAction(this, archerSkill, shotCooldown));

        // Послідовність для відступу
        SequenceNode retreatSequence = new SequenceNode();
        retreatSequence.Attach(new CheckDistanceToPlayer(this, minDistance, true));
        retreatSequence.Attach(new RetreatAction(this));

        // Послідовність для переслідування
        SequenceNode chaseSequence = new SequenceNode();
        chaseSequence.Attach(new CheckDistanceToPlayer(this, maxChaseDistance, true));
        chaseSequence.Attach(new Inverter(new CheckLineOfSight(this, obstacleLayer)));
        chaseSequence.Attach(new Inverter(new CheckDistanceToPlayer(this, attackRange, true)));
        chaseSequence.Attach(new ChaseAction(this, updatePathInterval));
        
        rootNode.Attach(chaseSequence);
        rootNode.Attach(attackSequence);
        rootNode.Attach(retreatSequence);

    }
}
