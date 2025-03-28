using UnityEngine;
// Перевірка дистанції до гравця
public class CheckDistanceToPlayer : EnemyBehaviorNode
{
    private float distance;
    private bool isCloser;

    public CheckDistanceToPlayer(BaseEnemyAI enemyAI, float checkDistance, bool checkIfCloser = true)
        : base(enemyAI)
    {
        distance = checkDistance;
        isCloser = checkIfCloser;
    }

    public override NodeState Evaluate()
    {
        float currentDistance = Vector2.Distance(enemy.transform.position, player.position);
        bool condition = isCloser ? currentDistance <= distance : currentDistance > distance;

        state = condition ? NodeState.Success : NodeState.Failure;
        return state;
    }
}