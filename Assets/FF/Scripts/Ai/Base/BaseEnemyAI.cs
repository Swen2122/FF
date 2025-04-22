using UnityEngine;
public abstract class BaseEnemyAI : MonoBehaviour, IEnemyAI
{
    [Header("Components")]
    public EnemyMovement enemyMove;
    public EnemySkillManager enemySkillManager;

    [SerializeField] protected StatusEffectManager statusEffectManager;

    [Header("Base Settings")]
    [SerializeField] protected float maxChaseDistance = 10f;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] public float updatePathInterval = 0.5f;

    protected Transform player;
    internal bool canMove = true;
    public Rigidbody2D rb;
    public Node rootNode;
    protected virtual void Start()
    {
        InitializeComponents();
        SetupBehaviorTree();
    }

    protected virtual void InitializeComponents()
    {
        statusEffectManager = gameObject.AddComponent<StatusEffectManager>();
        player = PlayerUtility.PlayerTransform;

        if (enemyMove != null)
        {
            enemyMove.OnPathCompleted += HandlePathCompleted;
        }

        //statusEffectManager.OnEffectAdded += HandleEffectAdded;
        //statusEffectManager.OnEffectRemoved += HandleEffectRemoved;
    }

    protected virtual void Update()
    {
        if (PauseManager.IsPaused) return;
        if (player == null) return;
        rootNode?.Evaluate();
    }

    protected abstract void SetupBehaviorTree();

    protected virtual void HandlePathCompleted()
    {
        float distanceToTarget = Vector2.Distance(transform.position, player.position);
        if (distanceToTarget > attackRange)
        {
            enemyMove.GetMoveCommand(player.position); 
        }
    }
    public virtual void EnableAI()
    {
        enabled = true;
        Debug.Log($"AI {gameObject.name} enabled"); 
    }

    public virtual void DisableAI()
    {
        enabled = false;
        enemyMove?.StopMoving();
        Debug.Log($"AI {gameObject.name} disabled"); 
    }

    protected virtual void OnDestroy()
    {
        if (enemyMove != null)
        {
            enemyMove.OnPathCompleted -= HandlePathCompleted;
        }

        if (statusEffectManager != null)
        {
            //statusEffectManager.OnEffectAdded -= HandleEffectAdded;
            //statusEffectManager.OnEffectRemoved -= HandleEffectRemoved;
        }
    }
}
