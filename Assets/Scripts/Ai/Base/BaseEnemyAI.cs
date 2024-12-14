using UnityEngine;

public abstract class BaseEnemyAI : MonoBehaviour, IEnemyAI
{
    [SerializeField] protected EnemyMovement enemyMove;
    [SerializeField] protected Transform player;

    public State currentState = State.Idle;

    [Header("Movement Settings")]
    public float updatePathInterval = 0.5f;
    public float minDistance = 2f;
    public float maxChaseDistance = 10f;

    [Header("Attack Settings")]
    public float attackRange = 1.5f;
    public float attackCooldown = 1f;

    protected float lastPathUpdateTime;
    protected float lastAttackTime;

    protected virtual void Start()
    {
        player = PlayerUtility.PlayerTransform;
    }

    protected virtual void Update()
    {
        UpdateState();
        HandleCurrentState();
    }

    protected virtual void UpdateState()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        currentState = distanceToPlayer switch
        {
            var d when d <= attackRange => State.Attack,
            var d when d <= maxChaseDistance => State.Chase,
            _ => State.Idle
        };
    }

    protected abstract void HandleCurrentState();
    protected abstract void Chase();
    protected abstract void Attack();

    public virtual void EnableAI() => enabled = true;
    public virtual void DisableAI() => enabled = false;

    protected bool CanReachTarget(float maxDistance, LayerMask obstacleLayer)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            transform.position, 
            player.position - transform.position, 
            maxDistance, 
            obstacleLayer
        );
        return hit.collider == null;
    }
}
