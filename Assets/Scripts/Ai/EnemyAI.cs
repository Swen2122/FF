using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : BaseEnemyAI
{
    [SerializeField] private Animator anim;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private Collider2D MeleeAttack;
    [SerializeField] private Element enemyElemet;
    private HashSet<GameObject> _player = new HashSet<GameObject>();

    protected override void ExecuteStateAction()
    {
        switch (currentState)
        {
            case State.Idle:
                // Ворог просто стоїть в Idle
                break;
            case State.Chase:
                // Використовуємо базову логіку переслідування
                base.OnChaseState();
                break;
            case State.Attack:
                PerformAttack();
                break;
        }
    }

    protected override void OnAttackState()
    {
        anim.SetTrigger("Attack");
        Debug.Log("Ворог атакує гравця!");
    }

    private void PerformAttack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("Ворог атакує гравця!");
    }

    // Цей метод викликається з анімації
    public void EnemyAttack()
    {
        _player = FindUtility.FindEnemy(MeleeAttack, targetLayer);
        HitStop.TriggerStop(0.05f, 0.0f);
        Damage.ApplyDamage(new List<GameObject>(_player).ToArray(), damage, enemyElemet);

        foreach (GameObject enemy in _player)
        {
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                PushUtility.Push(enemyRb, transform.position, 10f);
            }
        }
    }
}
