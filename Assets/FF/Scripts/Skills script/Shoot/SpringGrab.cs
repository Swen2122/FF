using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpringGrab : MonoBehaviour, IBulletBehavior
{
    public LayerMask targetLayerMask;   // ����� ��� ���������� �����
    public LayerMask boomLayerMask;   // ���� ��� ������� � ����� ���� �������� ��'���
    public float damageRadius = 2f;      // ����� �������� �����

    private HashSet<Rigidbody2D> connected = new HashSet<Rigidbody2D>();
    private Sequence moveSequence;

    private int bulletDamage;
    private bool isAttached = false; // ��������, �� ����������� ��'���
    public void SetBulletProperties(int damage)
    {
        bulletDamage = damage;
    }

    public void SetSequence(Sequence sequence)
    {
        moveSequence = sequence;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Rigidbody2D otherRigidbody = other.attachedRigidbody;
        if (((1 << other.gameObject.layer) & targetLayerMask) != 0 && otherRigidbody != null && !connected.Contains(otherRigidbody))
        {
            // ��������� FixedJoint2D
            FixedJoint2D fixedJoint = gameObject.AddComponent<FixedJoint2D>();
            fixedJoint.breakForce = Mathf.Infinity; // ��� �������
            fixedJoint.breakTorque = Mathf.Infinity; // ��� �������
            // �'������ � ����� ��'�����
            fixedJoint.connectedBody = otherRigidbody;
            fixedJoint.autoConfigureConnectedAnchor = false;

            // ������ � ������ ���������� ��
            connected.Add(otherRigidbody);
            isAttached = true; // ������������, �� ��'��� �����������
        }
    }

    private void Update()
    {
        if(PauseManager.IsPaused) return;
        // �������� �� ���������� �������� ��� �������� � Tilemap
        if (isAttached && moveSequence != null && !moveSequence.IsActive())
        {
            ApplyDamage();
            Destroy(gameObject); // ������� ��'��� ���� ������������ �����
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� ��'��� �������� � Tilemap
        if (((1 << collision.gameObject.layer) & boomLayerMask) != 0)
        {
            moveSequence.Kill();
            ApplyDamage();
            Destroy(gameObject); // ������� ��'��� ���� ������������ �����
        }
    }
    private void ApplyDamage()
    {
        // ������� ����� ��� ������� � ����
        Collider2D[] enemiesHit = Physics2D.OverlapCircleAll(transform.position, damageRadius, targetLayerMask);

        foreach (var enemy in enemiesHit)
        {
            // ����������, �� ��'��� ������� ����
            if (LaymaskUtility.InIsLMask(enemy.gameObject, targetLayerMask))
            {
                // �������� ���������, ���� ������ ICanHit
                if (enemy.TryGetComponent<ICanHit>(out var hitTarget))
                {
                    hitTarget.TakeHit(bulletDamage, Element.None); // ������� ����� ����� ���������
                }
            }
        }
    }

}
