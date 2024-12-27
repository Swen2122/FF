using UnityEngine;

public abstract class BaseEnemyAI : MonoBehaviour, IEnemyAI
{
    [SerializeField] protected EnemyMovement enemyMove;
    [SerializeField] protected float updatePathInterval = 0.5f;
    [SerializeField] protected float maxChaseDistance = 10f;
    [SerializeField] protected float attackRange = 1.5f;

    protected Transform player;
    protected float lastPathUpdateTime;
    protected State currentState = State.Idle;

    protected virtual void Start()
    {
        player = PlayerUtility.PlayerTransform;
    }

    protected virtual void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
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
            enemyMove.GetMoveCommand(player.position);
            lastPathUpdateTime = Time.time;
        }
    }
    protected abstract void OnAttackState();

    protected virtual bool CanAttackPlayer()
    {
        return true;
    }

    public virtual void EnableAI()
    {
        this.enabled = true;
    }

    public virtual void DisableAI()
    {
        this.enabled = false;
    }
}