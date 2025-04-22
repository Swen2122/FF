using UnityEngine;

public class EnemyBehaviorTree : BaseEnemyAI
{
    [SerializeField] protected float attackCooldown = 1f;

    protected override void Start()
    {
        base.Start();
        SetupBehaviorTree();
    }

    protected override void SetupBehaviorTree()
    {
        // ��������� �������� ��������
        rootNode = new Selector();

        // ��������� ������������ ��� �����
        SequenceNode attackSequence = new SequenceNode();
        attackSequence.Attach(new CheckDistanceToPlayer(this, attackRange, true));
        attackSequence.Attach(new AttackAction(this, attackCooldown));

        // ��������� ������������ ��� �������������
        SequenceNode chaseSequence = new SequenceNode();
        chaseSequence.Attach(new CheckDistanceToPlayer(this, maxChaseDistance, true));
        chaseSequence.Attach(new Inverter(new CheckDistanceToPlayer(this, attackRange, true)));
        chaseSequence.Attach(new ChaseAction(this, updatePathInterval));

        // ������ ����������� �� ���������� ���������
        rootNode.Attach(attackSequence);
        rootNode.Attach(chaseSequence);
    }
    protected override void Update()
    {
        if (player == null) return;
        rootNode.Evaluate();
    }
}
