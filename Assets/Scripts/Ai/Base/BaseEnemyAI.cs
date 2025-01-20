using UnityEngine;
// Базовий клас для всіх ворогів
public abstract class BaseEnemyAI : MonoBehaviour, IEnemyAI
{
    [Header("Components")]
    [SerializeField] public EnemyMovement enemyMove;
    [SerializeField] protected StatusEffectManager statusEffectManager;

    [Header("Base Settings")]
    [SerializeField] protected float maxChaseDistance = 10f;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] public float updatePathInterval = 0.5f;

    protected Transform player;
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

        statusEffectManager.OnEffectAdded += HandleEffectAdded;
        statusEffectManager.OnEffectRemoved += HandleEffectRemoved;
    }

    protected virtual void Update()
    {
        if (player == null) return;
        rootNode?.Evaluate();
    }

    // Метод для налаштування дерева поведінки - перевизначається в нащадках
    protected abstract void SetupBehaviorTree();

    protected virtual void HandlePathCompleted()
    {
        // Базова обробка завершення шляху
        float distanceToTarget = Vector2.Distance(transform.position, player.position);
        if (distanceToTarget > attackRange)
        {
            enemyMove.GetMoveCommand(player.position);
        }
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
        enemyMove?.StopMoving();
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
