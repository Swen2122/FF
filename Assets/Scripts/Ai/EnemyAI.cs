using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour, IEnemyAI
{
    [SerializeField] private Animator anim;
    [SerializeField] private EnemyMovement enemyMove;
    public State currentState = State.Idle; // ���������� ���� � Idle

    private Transform player; // ֳ��, ���� � �������
    public float attackRange = 1.5f; // ĳ������ �����
    public float maxChaseDistance = 10f; // ����������� �������, �� ��� ����� �������� ������
    public float updatePathInterval = 0.2f; // �������� ��������� �����

    private float lastPathUpdateTime; // ��� ���������� ��������� �����

    [Header("Attack Settings")]
    public int damage;
    private HashSet<GameObject> _player = new HashSet<GameObject>();  // HashSet ��� ��������� ��������
    public LayerMask targetLayer;
    public Collider2D MeleeAttack;
    void Start()
    {
        player = PlayerUtility.PlayerTransform;

        if (player != null)
        {
            Debug.Log("Transform ������ ������ ��������: " + player.position);
        }
        else
        {
            Debug.LogError("�� ������� �������� Transform ������!");
        }
    }
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // ���� ����� �������� �� ������ �� ������
        if (distanceToPlayer <= attackRange)
        {
            currentState = State.Attack; // ���� ����� � ��� �����, �� �����
        }
        else if (distanceToPlayer <= maxChaseDistance)
        {
            currentState = State.Chase; // ���� ������� ��������, ����� ������ ������������
        }
        else
        {
            currentState = State.Idle; // ���� ������� ������� ������, ����� ���������� �� ����
        }

        // ��������� �� �������� �� ��������� �����
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
        // ��������� ���� �� ������ �� ������� ����������
        if (Time.time - lastPathUpdateTime > updatePathInterval)
        {
            Vector2 playes_position = new Vector2(player.position.x, player.position.y);
            enemyMove.GetMoveCommand(playes_position);
            // ��������� ���� ��� ����
            lastPathUpdateTime = Time.time;
        }
    }

    void Attack()
    {
        anim.SetTrigger("Attack");
        Debug.Log("����� ����� ������!");
    }

    public void EnemyAttack()
    {
        _player = FindUtility.FindEnemy(MeleeAttack, targetLayer);
        HitStop.TriggerStop(0.05f, 0.0f);
        Damage.Earth(new List<GameObject>(_player).ToArray(), damage);
        foreach (GameObject enemy in _player)
        {
            // �������� Rigidbody2D ������ ��� ������������ ������
            Rigidbody2D enemyRb = enemy.GetComponent<Rigidbody2D>();

            // ���� ����� �� Rigidbody2D
            if (enemyRb != null)
            {
                // ������������� ����� Push ��� ������������� ������
                PushUtility.Push(enemyRb, transform.position, 10f);
            }
        }
    }
}

[System.Serializable]
public enum State { Idle, Attack, Chase}
