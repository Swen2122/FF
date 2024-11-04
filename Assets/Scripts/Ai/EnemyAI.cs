using UnityEngine;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] EnemyMovement enemyMove;
    public State currentState = State.Idle; // ���������� ���� � Idle

    public Transform player; // ֳ��, ���� � �������
    public float attackRange = 1.5f; // ĳ������ �����
    public float maxChaseDistance = 10f; // ����������� �������, �� ��� ����� �������� ������
    public float updatePathInterval = 0.2f; // �������� ��������� �����

    private float lastPathUpdateTime; // ��� ���������� ��������� �����

    [Header("Attack Settings")]
    public int damage;
    private HashSet<GameObject> _player = new HashSet<GameObject>();  // HashSet ��� ��������� ��������
    public LayerMask targetLayer;
    public Collider2D MeleeAttack;
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
        Damage.Earth(new List<GameObject>(_player).ToArray(), damage);
    }
}

[System.Serializable]
public enum State { Idle, Attack, Chase }
