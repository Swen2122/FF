using UnityEngine;
// Дія переслідування
public class ChaseAction : EnemyBehaviorNode
{
    private float updateInterval;
    private float lastUpdateTime;

    public ChaseAction(BaseEnemyAI enemyAI, float interval = 0.5f) : base(enemyAI)
    {
        updateInterval = interval;
    }

    public override NodeState Evaluate()
    {
        if (Time.time - lastUpdateTime > updateInterval)
        {
            enemy.enemyMove.GetMoveCommand(player.position);
            lastUpdateTime = Time.time;
        }

        state = NodeState.Running;
        return state;
    }
}
