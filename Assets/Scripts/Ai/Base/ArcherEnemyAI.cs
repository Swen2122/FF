using UnityEngine;

public class ArcherEnemyAI : BaseEnemyAI
{
    [SerializeField] private ArcherConfiguration archerConfig;
    [SerializeField] private Archer_Skill archerSkill;

    [Header("Archer Specific Settings")]
    [SerializeField] private float retreatDistance = 1f;
    [SerializeField] private LayerMask obstacleLayer;

    protected override void UpdateState()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        currentState = distanceToPlayer switch
        {
            var d when d <= attackRange && CanReachTarget(attackRange, obstacleLayer) => State.Attack,
            var d when d > minDistance && d <= maxChaseDistance => State.Chase,
            _ => State.Idle
        };
    }

    protected override void HandleCurrentState()
    {
        switch (currentState)
        {
            case State.Idle:
                enemyMove.StopMoving();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    protected override void Chase()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (Time.time - lastPathUpdateTime > updatePathInterval)
        {
            if (distanceToPlayer >= archerConfig.optimalAttackDistance)
            {
                enemyMove.GetMoveCommand(player.position);
                lastPathUpdateTime = Time.time;
            }
            else if (distanceToPlayer < archerConfig.optimalAttackDistance)
            {
                // Відступ від гравця
                Vector2 retreatDirection = (transform.position - player.position).normalized;
                Vector2 retreatPosition = (Vector2)transform.position + retreatDirection * retreatDistance;
                enemyMove.GetMoveCommand(retreatPosition);
            }
        }
    }

    protected override void Attack()
    {
        if (Time.time - lastAttackTime >= archerConfig.attackCooldown)
        {
            lastAttackTime = Time.time;
            Shoot();
        }
    }

    private void Shoot()
    {
        archerSkill.BoltShot(player, archerConfig.projectileSpeed, archerConfig.projectileDamage);
    }
}

[System.Serializable]
public class ArcherConfiguration
{
    public float optimalAttackDistance = 6f;
    public float attackCooldown = 3f;
    public float projectileSpeed = 30f;
    public int projectileDamage = 30;
}
