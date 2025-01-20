using UnityEngine;
using System.Collections.Generic;
// Базовий клас для ближнього бою
public class MeleeEnemyAI : BaseEnemyAI
{
    [Header("Melee Settings")]
    [SerializeField] private Animator anim;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Collider2D meleeAttack;
    [SerializeField] private Element enemyElement;
    [SerializeField] private float attackCooldown = 1f;

    private HashSet<GameObject> hitTargets = new HashSet<GameObject>();

    protected override void SetupBehaviorTree()
    {
        rootNode = new Selector();

        // Послідовність для атаки
        SequenceNode attackSequence = new SequenceNode();
        attackSequence.Attach(new CheckDistanceToPlayer(this, attackRange, true));
        attackSequence.Attach(new MeleeAttackAction(this, anim, attackCooldown));

        // Послідовність для переслідування
        SequenceNode chaseSequence = new SequenceNode();
        chaseSequence.Attach(new CheckDistanceToPlayer(this, maxChaseDistance, true));
        chaseSequence.Attach(new Inverter(new CheckDistanceToPlayer(this, attackRange, true)));
        chaseSequence.Attach(new ChaseAction(this, updatePathInterval));

        rootNode.Attach(attackSequence);
        rootNode.Attach(chaseSequence);
    }

    // Викликається з анімації
    public void ExecuteMeleeAttack()
    {
        hitTargets = FindUtility.FindEnemy(meleeAttack, targetLayer);
        HitStop.TriggerStop(0.05f, 0.0f);
        Damage.ApplyDamage(new List<GameObject>(hitTargets).ToArray(), damage, enemyElement);

        foreach (GameObject target in hitTargets)
        {
            if (target.TryGetComponent<Rigidbody2D>(out var targetRb))
            {
                PushUtility.Push(targetRb, transform.position, 10f);
            }
        }
    }
}
