using UnityEngine;
using System.Collections.Generic;

public class MeleeEnemyAI : BaseEnemyAI
{
    [SerializeField] private MeleeEnemyConfiguration meleeConfig;
    [SerializeField] private Animator animator;
    [SerializeField] private Collider2D meleeAttackCollider;

    private HashSet<GameObject> targetsHit = new HashSet<GameObject>();

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
        if (Time.time - lastPathUpdateTime > updatePathInterval)
        {
            enemyMove.GetMoveCommand(player.position);
            lastPathUpdateTime = Time.time;
        }
    }

    protected override void Attack()
    {
        if (Time.time - lastAttackTime >= meleeConfig.attackCooldown)
        {
            lastAttackTime = Time.time;
            PerformMeleeAttack();
        }
    }

    private void PerformMeleeAttack()
    {
        animator.SetTrigger("Attack");
        
        // Пошук цілей в зоні атаки
        targetsHit = FindUtility.FindEnemy(meleeAttackCollider, meleeConfig.targetLayer);
        
        // Зупинка часу при ударі
        HitStop.TriggerStop(0.05f, 0.0f);
        
        // Нанесення шкоди
        Damage.Earth(new List<GameObject>(targetsHit).ToArray(), meleeConfig.damage);
        
        // Відштовхування цілей
        foreach (GameObject target in targetsHit)
        {
            Rigidbody2D targetRb = target.GetComponent<Rigidbody2D>();
            if (targetRb != null)
            {
                PushUtility.Push(targetRb, transform.position, meleeConfig.pushForce);
            }
        }
    }
}

[System.Serializable]
public class MeleeEnemyConfiguration
{
    public float attackCooldown = 1f;
    public int damage = 10;
    public float pushForce = 10f;
    public LayerMask targetLayer;
}
