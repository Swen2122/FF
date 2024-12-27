
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour, IBulletBehavior
{
    [SerializeField] private LayerMask targets;
    private int bulletDamage;
    private Sequence moveSequence;
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
        // ������������� LaymaskUtility ��� �������� ����
        if (LaymaskUtility.InIsLMask(other.gameObject, targets))
        {
            // �������� ��'���, ���� ������ ICanHit
            if (other.gameObject.TryGetComponent<ICanHit>(out var hitTarget))
            {
                hitTarget.TakeHit(bulletDamage); // ��������� TakeDMG ����� ���������
            }

            // ��������� ��� ��� �� ������� ��
            if (moveSequence != null)
            {
                moveSequence.Kill();
            }
            Destroy(gameObject);
        }
    }
}
