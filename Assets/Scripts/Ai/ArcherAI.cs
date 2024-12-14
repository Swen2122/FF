using UnityEngine;

public class ArcherAI : MonoBehaviour, IEnemyAI
{
    [SerializeField] private EnemyMovement enemyMove;
    [SerializeField] private float updatePathInterval = 0.6f;
    private float lastPathUpdateTime;

    [SerializeField] private Transform player;
    [SerializeField] private float retreatDistance = 1f;
    [SerializeField] private float minDistance = 4f;
    [SerializeField] private float attackRange = 7f;
    [SerializeField] private float maxChaseDistance = 10f;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float checkAttackInterval = 0.5f;
    [SerializeField] private float shotCooldown = 3f;
    [SerializeField] private Archer_Skill arch;
    private float lastAttackTime;
    private float lastCheckTime;

    public State currentState = State.Idle;
    void Start()
    {
        player = PlayerUtility.PlayerTransform;
    }
    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > maxChaseDistance || distanceToPlayer < minDistance)
        {
            currentState = State.Idle;
            enemyMove.StopMoving(); // «упин€Їмо рух у стан≥ Idle
        }
        else
        {
            currentState = State.Chase;
        }

        if (Time.time - lastCheckTime > checkAttackInterval)
        {
            lastCheckTime = Time.time;
            if (distanceToPlayer <= attackRange && CanAttackPlayer())
            {
                currentState = State.Attack;
            }else if (!CanAttackPlayer())
            {
                currentState = State.Chase;
            }
        }

        switch (currentState)
        {
            case State.Idle:
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                AttemptAttack();
                break;
        }
    }

    void Chase()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer >= minDistance && Time.time - lastPathUpdateTime > updatePathInterval )
        {
            if (!enemyMove.IsMoving()) // ѕерев≥р€Їмо, чи ворог уже не рухаЇтьс€
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

    private bool CanAttackPlayer()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.position - transform.position, attackRange, obstacleLayer);
        return hit.collider == null;
    }

    private void AttemptAttack()
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
    public void EnableAI()
    {
        this.enabled = true;
    }
    public void DisableAI()
    {
        this.enabled = false;
    }
}
