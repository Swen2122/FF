using UnityEngine;
public class RetreatAction : EnemyBehaviorNode
{
    private float lastUpdateTime;
    private const float updateInterval = 0.5f;

    public RetreatAction(BaseEnemyAI enemyAI) : base(enemyAI) { }

    public override NodeState Evaluate()
    {
        if (Time.time - lastUpdateTime > updateInterval)
        {
            Vector2 directionToPlayer = (enemy.transform.position - player.position).normalized;
            Vector2 retreatPosition = (Vector2)enemy.transform.position + directionToPlayer * 2f;

            enemy.enemyMove.GetMoveCommand(retreatPosition);
            lastUpdateTime = Time.time;
        }

        state = NodeState.Running;
        return state;
    }
}