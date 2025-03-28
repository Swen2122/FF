using UnityEngine;

public class DashAction : EnemyBehaviorNode
{
    private float dashCooldown;
    private float lastDashTime;
    private float dashDistance;
    private float dashSpeed;
    private Vector3 dashDirection;
    private Vector3 startPosition;
    private bool isDashing = false;

    public DashAction(BaseEnemyAI enemyAI, DashConfig dashConfig) : base(enemyAI)
    {
        dashCooldown = dashConfig.dashCooldown;
        dashDistance = dashConfig.dashDistance;
        dashSpeed = dashConfig.dashSpeed;
    }

    public override NodeState Evaluate()
    {
        if (isDashing)
        {
            float distanceTraveled = Vector3.Distance(startPosition, enemy.transform.position);
            
            if (distanceTraveled >= dashDistance)
            {
                enemy.rb.linearVelocity = Vector2.zero; // Зупиняємо ривок
                isDashing = false;
                state = NodeState.Success;
                return state;
            }
            
            state = NodeState.Running;
            return state;
        }

        if (Time.time - lastDashTime >= dashCooldown)
        {
            PerformDash();
            lastDashTime = Time.time;
            isDashing = true;
            state = NodeState.Running;
        }
        else
        {
            state = NodeState.Running;
        }

        return state;
    }

    protected void PerformDash()
    {
        startPosition = enemy.transform.position;
        dashDirection = (player.position - startPosition).normalized;
        enemy.rb.linearVelocity = dashDirection * dashSpeed;
    }
}