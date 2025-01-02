using UnityEngine;
public abstract class BaseEnemyAI : MonoBehaviour, IEnemyAI
{
    [SerializeField] protected EnemyMovement enemyMove;
    [SerializeField] protected StatusEffectManager statusEffectManager;
    [SerializeField] public float updatePathInterval = 0.5f;
    [SerializeField] protected float maxChaseDistance = 10f;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected float targetPositionUpdateInterval = 0.1f;

    protected Transform player;
    protected float lastPathUpdateTime;
    protected float lastTargetUpdateTime;
    protected State currentState = State.Idle;
    protected Vector2 lastKnownPlayerPosition;

    protected virtual void Start()
    {
        statusEffectManager = gameObject.AddComponent<StatusEffectManager>();
        player = PlayerUtility.PlayerTransform;
        lastKnownPlayerPosition = player.position;

        if (enemyMove != null)
        {
            enemyMove.OnPathCompleted += HandlePathCompleted;
        }

        statusEffectManager.OnEffectAdded += HandleEffectAdded;
        statusEffectManager.OnEffectRemoved += HandleEffectRemoved;
    }

    protected virtual void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        UpdateTargetPosition();
        UpdateState(distanceToPlayer);
        ExecuteStateAction();
    }

    protected virtual void UpdateState(float distanceToPlayer)
    {
        if (distanceToPlayer <= attackRange && CanAttackPlayer())
        {
            currentState = State.Attack;
        }
        else if (distanceToPlayer <= maxChaseDistance)
        {
            currentState = State.Chase;
        }
        else
        {
            currentState = State.Idle;
            enemyMove.StopMoving();
        }
    }

    protected virtual void UpdateTargetPosition()
    {
        if (Time.time - lastTargetUpdateTime >= targetPositionUpdateInterval)
        {
            Vector2 currentPlayerPosition = player.position;
            float positionChange = Vector2.Distance(lastKnownPlayerPosition, currentPlayerPosition);

            if (positionChange > 0.1f)
            {
                lastKnownPlayerPosition = currentPlayerPosition;
                if (currentState == State.Chase)
                {
                    enemyMove.SetTarget(currentPlayerPosition);
                }
            }

            lastTargetUpdateTime = Time.time;
        }
    }

    protected virtual void HandlePathCompleted()
    {
        if (currentState == State.Chase)
        {
            float distanceToTarget = Vector2.Distance(transform.position, lastKnownPlayerPosition);
            if (distanceToTarget > attackRange)
            {
                enemyMove.GetMoveCommand(player.position);
            }
        }
    }

    protected virtual void ExecuteStateAction()
    {
        switch (currentState)
        {
            case State.Idle:
                OnIdleState();
                break;
            case State.Chase:
                OnChaseState();
                break;
            case State.Attack:
                OnAttackState();
                break;
        }
    }

    protected virtual void OnIdleState() { }

    protected virtual void OnChaseState()
    {
        if (Time.time - lastPathUpdateTime > updatePathInterval)
        {
            Vector2 targetPosition = player.position;
            enemyMove.GetMoveCommand(targetPosition);
            lastPathUpdateTime = Time.time;
            lastKnownPlayerPosition = targetPosition;
        }
    }

    protected abstract void OnAttackState();

    protected virtual bool CanAttackPlayer()
    {
        return true;
    }

    protected virtual void HandleEffectAdded(string effectId)
    {
        Debug.Log($"Effect {effectId} added to {gameObject.name}");
    }

    protected virtual void HandleEffectRemoved(string effectId)
    {
        Debug.Log($"Effect {effectId} removed from {gameObject.name}");
    }

    public void AddStatusEffect(IStatusEffect effect)
    {
        statusEffectManager?.AddEffect(effect);
    }

    public virtual void EnableAI()
    {
        this.enabled = true;
    }

    public virtual void DisableAI()
    {
        this.enabled = false;
    }

    protected virtual void OnDestroy()
    {
        if (enemyMove != null)
        {
            enemyMove.OnPathCompleted -= HandlePathCompleted;
        }

        if (statusEffectManager != null)
        {
            statusEffectManager.OnEffectAdded -= HandleEffectAdded;
            statusEffectManager.OnEffectRemoved -= HandleEffectRemoved;
        }
    }
}
public enum State
{
    Idle,
    Attack,
    Chase
}