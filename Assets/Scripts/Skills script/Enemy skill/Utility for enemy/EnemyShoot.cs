using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyShoot : MonoBehaviour
{
    public Transform shootPoint; // Точка, з якої відбувається постріл
    private Sequence moveBullet;

    /// <summary>
    /// Запуск снаряда
    /// </summary>
    /// <param name="bulletPrefab">Префаб кулі</param>
    /// <param name="target">Ціль</param>
    /// <param name="speed">Швидкість</param>
    /// <param name="damage">Шкода</param>
    /// <param name="lifetime">Час життя снаряда</param>
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

        // Створення кулі на позиції shootPoint
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);

        // Обчислення напрямку від shootPoint до target
        Vector3 direction = (target.position - shootPoint.position).normalized;

        // Корекція обертання кулі
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        // Розрахунок кінцевої позиції на основі швидкості та дистанції
        Vector3 finalPosition = shootPoint.position + direction * (speed * lifetime);

        // Запуск tween для переміщення кулі
        moveBullet = DOTween.Sequence();
        moveBullet.Append(bullet.transform.DOMove(finalPosition, lifetime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Destroy(bullet);
                Debug.Log("Bullet expired after traveling calculated distance.");
            }));

        // Налаштування параметрів кулі через IBulletBehavior
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
