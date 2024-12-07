using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bolt : MonoBehaviour, IBulletBehavior
{
    public LayerMask targetLayerMask;   // ����� ��� ���������� �����
    public LayerMask pierceLayerMask;   // ���� ��� ������� � ����� ���� �������� ��'���

    private HashSet<Rigidbody2D> connected = new HashSet<Rigidbody2D>();
    private Sequence moveSequence;

    private int bulletDamage;
    private bool isAttached = false;
    public void SetBulletProperties(int damage)
    {
        bulletDamage = damage;
    }

    public void SetSequence(Sequence sequence)
    {
        moveSequence = sequence;
    }
    private void Update()
    {
        // �������� �� ���������� ������� ��� �������� � Tilemap
        if (isAttached && moveSequence != null && !moveSequence.IsActive())
        {
            Destroy(gameObject); // ������� ��'��� ���� ������������ �����
        }
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
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // ���� ��'��� �������� � Tilemap
        if (((1 << collision.gameObject.layer) & pierceLayerMask) != 0)
        {
            moveSequence.Kill();
            isAttached = true;
        }
    }
}
