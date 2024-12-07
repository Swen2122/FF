using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour, IEnemyAI
{
    [SerializeField] private Animator anim;
    [SerializeField] private EnemyMovement enemyMove;
    public State currentState = State.Idle; // ѕочатковий стан Ч Idle

    private Transform player; // ÷≥ль, €кою Ї гравець
    public float attackRange = 1.5f; // ƒ≥апазон атаки
    public float maxChaseDistance = 10f; // ћаксимальна в≥дстань, на €к≥й ворог пересл≥дуЇ гравц€
    public float updatePathInterval = 0.2f; // ≤нтервал оновленн€ шл€ху

    private float lastPathUpdateTime; // „ас останнього оновленн€ шл€ху

    [Header("Attack Settings")]
    public int damage;
    private HashSet<GameObject> _player = new HashSet<GameObject>();  // HashSet дл€ уникненн€ дубл≥кат≥в
    public LayerMask targetLayer;
    public Collider2D MeleeAttack;
    void Start()
    {
        player = PlayerUtility.PlayerTransform;

        if (player != null)
        {
            Debug.Log("Transform гравц€ усп≥шно присвоЇно: " + player.position);
        }
        else
        {
            Debug.LogError("Ќе вдалос€ отримати Transform гравц€!");
        }
    }
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ¬иб≥р стану в≥дпов≥дно до в≥дстан≥ до гравц€
        if (distanceToPlayer <= attackRange)
        {
            currentState = State.Attack; // якщо ворог в зон≥ атаки, в≥н атакуЇ
        }
        else if (distanceToPlayer <= maxChaseDistance)
        {
            currentState = State.Chase; // якщо гравець недалеко, ворог починаЇ пересл≥дувати
        }
        else
        {
            currentState = State.Idle; // якщо гравець занадто далеко, ворог залишаЇтьс€ на м≥сц≥
        }

        // ¬иконанн€ д≥й в≥дпов≥дно до поточного стану
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
    public void EnableAI()
    {
        this.enabled = true;
    }
    public void DisableAI()
    {
        this.enabled = false;
    }
    void Chase()
    {
        // ќновлюЇмо шл€х до гравц€ ≥з заданим ≥нтервалом
        if (Time.time - lastPathUpdateTime > updatePathInterval)
        {
            Vector2 playes_position = new Vector2(player.position.x, player.position.y);
            enemyMove.GetMoveCommand(playes_position);
            // ќновлюЇмо ц≥ль дл€ руху
            lastPathUpdateTime = Time.time;
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("¬орог атакуЇ гравц€!");
    }

    public void EnemyAttack()
    {
        _player = FindUtility.FindEnemy(MeleeAttack, targetLayer);
        HitStop.TriggerStop(0.05f, 0.0f);
        Damage.Earth(new List<GameObject>(_player).ToArray(), damage);
        foreach (GameObject enemy in _player)
        {
            // ќтримуЇмо Rigidbody2D ворога дл€ застосуванн€ ф≥зики
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();

            // якщо ворог маЇ Rigidbody2D
            if (enemyRb != null)
            {
                // ¬икористовуЇмо метод Push дл€ в≥дштовхуванн€ ворога
                PushUtility.Push(enemyRb, transform.position, 10f);
            }
        }
    }
}

[System.Serializable]
public enum State { Idle, Attack, Chase}
