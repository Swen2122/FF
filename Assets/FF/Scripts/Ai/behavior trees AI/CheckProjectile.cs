using UnityEngine;

public class CheckProjectile : EnemyBehaviorNode
{
    private float radius;
    private LayerMask projectileLayer;
    public CheckProjectile(BaseEnemyAI enemyAI, float radius, LayerMask projectileLayer): base(enemyAI)
    {
        this.radius = radius;
        this.projectileLayer = projectileLayer;
    }
    public override NodeState Evaluate()
    {
        if (enemy == null || enemy.transform == null)
        {
            Debug.LogError("Enemy або його Transform не ініціалізовано!");
            state = NodeState.Failure;
            return state;
        }

        Collider2D colliders = Physics2D.OverlapCircle(enemy.transform.position, radius, projectileLayer);
        if (colliders != null && colliders.TryGetComponent<BaseProjectile>(out var projectile))
        {
            state = NodeState.Success;
            return state;
        }
        else
        {
            state = NodeState.Failure;
            return state;
        }
    }
}
