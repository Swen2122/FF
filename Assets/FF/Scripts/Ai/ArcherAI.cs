using UnityEngine;

// ��������� �������
public class ArcherAI : BaseEnemyAI
{
    [Header("Archer Settings")]
    [SerializeField] private float minDistance = 4f;
    [SerializeField] private LayerMask obstacleLayer;

    protected override void SetupBehaviorTree()
    {
        rootNode = new Selector();

        SequenceNode attackSequence = new SequenceNode();
            attackSequence.Attach(new CheckDistanceToPlayer(this, attackRange, true));
            attackSequence.Attach(new Inverter(new CheckDistanceToPlayer(this, minDistance, true)));
            attackSequence.Attach(new CheckLineOfSight(this, obstacleLayer));
            attackSequence.Attach(new UseSkillPosition(this, Controler.Instance.transform.position, TriggerType.damage));
        
        SequenceNode retreatSequence = new SequenceNode();
            retreatSequence.Attach(new CheckDistanceToPlayer(this, minDistance, true));
            retreatSequence.Attach(new RetreatAction(this));

        Selector chaseSelector = new Selector();

            SequenceNode chaseSequence = new SequenceNode();
                chaseSequence.Attach(new CheckDistanceToPlayer(this, maxChaseDistance, true));
                chaseSequence.Attach(new CheckLineOfSight(this, obstacleLayer));
                chaseSequence.Attach(new Inverter(new CheckDistanceToPlayer(this, attackRange, true)));
                chaseSequence.Attach(new ChaseAction(this, updatePathInterval));

            SequenceNode seekSequence = new SequenceNode();
                seekSequence.Attach(new CheckDistanceToPlayer(this, maxChaseDistance, true));
                seekSequence.Attach(new Inverter(new CheckLineOfSight(this, obstacleLayer)));
                seekSequence.Attach(new ChaseAction(this, updatePathInterval));
            chaseSelector.Attach(chaseSequence);
            chaseSelector.Attach(seekSequence);

        rootNode.Attach(chaseSelector);
        rootNode.Attach(attackSequence);
        rootNode.Attach(retreatSequence);

    }
}
