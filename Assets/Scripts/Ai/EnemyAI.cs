using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] EnemyMovement enemyMove;
    public State currentState = State.Idle; // Початковий стан — Idle

    public Transform player; // Ціль, якою є гравець
    public float attackRange = 1.5f; // Діапазон атаки
    public float maxChaseDistance = 10f; // Максимальна відстань, на якій ворог переслідує гравця
    public float updatePathInterval = 0.2f; // Інтервал оновлення шляху

    private float lastPathUpdateTime; // Час останнього оновлення шляху

    [Header("Attack Settings")]
    public int damage;
    private HashSet<GameObject> _player = new HashSet<GameObject>();  // HashSet для уникнення дублікатів
    public LayerMask targetLayer;
    public Collider2D MeleeAttack;
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Вибір стану відповідно до відстані до гравця
        if (distanceToPlayer <= attackRange)
        {
            currentState = State.Attack; // Якщо ворог в зоні атаки, він атакує
        }
        else if (distanceToPlayer <= maxChaseDistance)
        {
            currentState = State.Chase; // Якщо гравець недалеко, ворог починає переслідувати
        }
        else
        {
            currentState = State.Idle; // Якщо гравець занадто далеко, ворог залишається на місці
        }

        // Виконання дій відповідно до поточного стану
        switch (currentState)
        {
            case State.Idle:
                Idle();
                break;
            case State.Chase:
                Chase();
                break;
            case State.Attack:
                Attack();
                break;
        }
    }

    void Idle()
    {

    }

    void Chase()
    {
        // Оновлюємо шлях до гравця із заданим інтервалом
        if (Time.time - lastPathUpdateTime > updatePathInterval)
        {
            Vector2 playes_position = new Vector2(player.position.x, player.position.y);
            enemyMove.GetMoveCommand(playes_position);
            // Оновлюємо ціль для руху
            lastPathUpdateTime = Time.time;
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("Ворог атакує гравця!");
    }

    public void EnemyAttack()
    {
        _player = FindUtility.FindEnemy(MeleeAttack, targetLayer);
        Damage.Earth(new List<GameObject>(_player).ToArray(), damage);
    }
}

[System.Serializable]
public enum State { Idle, Attack, Chase }
