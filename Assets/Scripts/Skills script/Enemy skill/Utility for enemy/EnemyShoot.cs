using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyShoot : MonoBehaviour
{
    public Transform shootPoint; // �����, � ��� ���������� ������
    private Sequence moveBullet;

    /// <summary>
    /// ������ �������
    /// </summary>
    /// <param name="bulletPrefab">������ ���</param>
    /// <param name="target">ֳ��</param>
    /// <param name="speed">��������</param>
    /// <param name="damage">�����</param>
    /// <param name="lifetime">��� ����� �������</param>
    public void Bullet(GameObject bulletPrefab, Transform target, float speed, int damage, float lifetime)
    {
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab is not assigned!");
            return;
        }

        if (shootPoint == null)
        {
            Debug.LogWarning("Shoot point is not assigned!");
            return;
        }

        if (target == null)
        {
            Debug.LogWarning("Target is not assigned!");
            return;
        }

        // ��������� ��� �� ������� shootPoint
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // ���������� �������� �� shootPoint �� target
        Vector3 direction = (target.position - shootPoint.position).normalized;

        // �������� ��������� ���
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        // ���������� ������ ������� �� ����� �������� �� ���������
        Vector3 finalPosition = shootPoint.position + direction * (speed * lifetime);

        // ������ tween ��� ���������� ���
        moveBullet = DOTween.Sequence();
        moveBullet.Append(bullet.transform.DOMove(finalPosition, lifetime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Destroy(bullet);
                Debug.Log("Bullet expired after traveling calculated distance.");
            }));

        // ������������ ��������� ��� ����� IBulletBehavior
        IBulletBehavior bulletBehavior = bullet.GetComponent<IBulletBehavior>();
        if (bulletBehavior != null)
        {
            bulletBehavior.SetBulletProperties(damage);
            bulletBehavior.SetSequence(moveBullet);
        }
        else
        {
            Debug.LogError("No IBulletBehavior found on the bullet prefab!");
            Destroy(bullet);
        }
    }
}
