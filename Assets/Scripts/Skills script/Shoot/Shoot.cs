using UnityEngine;
using DG.Tweening;

public class Shoot : MonoBehaviour
{
    public Transform shootPoint;
    public Camera mainCamera;
    public Sequence moveBullet;

    private void Awake()
    {
        // Кешуємо основну камеру для оптимізації
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    // Функція для стрільби з параметрами швидкості, шкоди та тривалості життя
    public void Bullet(GameObject bulletPrefab, float speed, int damage, float lifetime)
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

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Створення кулі на позиції `shootPoint`
        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Quaternion.identity);
        // Обчислення напрямку і повороту кулі
        Vector3 direction = (mousePosition - shootPoint.position).normalized;
        bullet.transform.up = direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle - 90);

        // Запуск tween для переміщення кулі
        Vector3 targetPos = shootPoint.position + direction * speed * lifetime;
        moveBullet = DOTween.Sequence();
        moveBullet.Append(bullet.transform.DOMove(targetPos, lifetime)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                Destroy(bullet);
                Debug.Log("Bullet reached target");
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
