using UnityEngine;

public class ArcherAI : BaseEnemyAI
{
    [SerializeField] private float retreatDistance = 1f;
    [SerializeField] private float minDistance = 4f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float checkAttackInterval = 0.5f;
    [SerializeField] private float shotCooldown = 3f;
    [SerializeField] private Archer_Skill arch;

    private float lastAttackTime;
    private float lastCheckTime;

    protected override void UpdateState(float distanceToPlayer)
    {
        if (distanceToPlayer > maxChaseDistance || distanceToPlayer < minDistance)
        {
            currentState = State.Idle;
            enemyMove.StopMoving();
        }
        else if (Time.time - lastCheckTime > checkAttackInterval)
        {
            lastCheckTime = Time.time;
            if (distanceToPlayer <= attackRange && CanAttackPlayer())
            {
                currentState = State.Attack;
            }
            else
            {
                currentState = State.Chase;
            }
        }
    }

    protected override void ExecuteStateAction()
    {
        switch (currentState)
        {
            case State.Idle:
                // Лучник просто стоїть в Idle
                break;
            case State.Chase:
                ChasePlayer();
                break;
            case State.Attack:
                OnAttackState();  // Тепер викликаємо OnAttackState замість AttemptAttack
                break;
        }
    }

    private void ChasePlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer >= minDistance && Time.time - lastPathUpdateTime > updatePathInterval)
        {
            if (!enemyMove.IsMoving())
            {
                enemyMove.GetMoveCommand(player.position);
                lastPathUpdateTime = Time.time;
            }
        }
        else if (distanceToPlayer < minDistance)
        {
            enemyMove.StopMoving();
        }
    }

    protected override bool CanAttackPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, attackRange, obstacleLayer);
        return hit.collider == null;
    }

    protected override void OnAttackState()
    {
        if (Time.time - lastAttackTime >= shotCooldown)
        {
            lastAttackTime = Time.time;
            Shoot();
        }
    }

    private void Shoot()
    {
        arch.BoltShot(player, 30f, 30);
    }
}
