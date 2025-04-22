using UnityEngine;

// ������� ���� ��� �� ������
public abstract class EnemyBehaviorNode : Node
{
    protected BaseEnemyAI enemy;
    protected Transform player;

    public EnemyBehaviorNode(BaseEnemyAI enemyAI)
    {
        enemy = enemyAI;
        player = PlayerUtility.PlayerTransform;
    }

    public void Initialize(BaseEnemyAI enemyAI)
    {
        enemy = enemyAI;
        player = PlayerUtility.PlayerTransform;
    }
}