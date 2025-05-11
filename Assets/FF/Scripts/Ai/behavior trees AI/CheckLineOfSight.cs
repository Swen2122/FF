using UnityEngine;
// �������� ����� ��� �������
public class CheckLineOfSight : EnemyBehaviorNode
{
    private LayerMask obstacleLayer;

    public CheckLineOfSight(BaseEnemyAI enemyAI, LayerMask obstacles)
        : base(enemyAI)
    {
        obstacleLayer = obstacles;
    }

    public override NodeState Evaluate()
    {
        Vector2 direction = player.position - enemy.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(enemy.transform.position, direction, direction.magnitude, obstacleLayer);
        state = hit.collider == null ? NodeState.Success : NodeState.Failure;
        return state;
    }
}
