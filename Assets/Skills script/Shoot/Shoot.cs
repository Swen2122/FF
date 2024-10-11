using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shoot : MonoBehaviour
{
    
    public Transform shootPoint;
    public Sequence moveBullet;

    // Функція для стрільби з параметрами швидкості, шкоди та тривалості життя
    public void bullet(GameObject bullet_obg, float speed, int damage, float lifetime)
    {
        if (bullet_obg == null)
        {
            Debug.LogWarning("Bullet prefab is not assigned!");
            return;
        }
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        // Визначаємо позицію стрільби (можете замінити shootPoint на mousePosition)
        Vector3 shootPosition = transform.position; // позиція, з якої стріляєте
        GameObject bullet = Instantiate(bullet_obg, shootPosition, Quaternion.identity);
        // Встановлюємо напрямок
        Vector3 direction = (mousePosition - shootPosition).normalized;
        // Встановлюємо кут повороту кулі (для візуалізації напрямку)
        bullet.transform.up = direction;
        // Запускаємо переміщення кулі
        Vector3 targetPos = shootPosition + direction * speed * lifetime;
        moveBullet = DOTween.Sequence();
        moveBullet.Append(bullet.transform.DOMove(targetPos, lifetime).SetEase(Ease.Linear).OnComplete(() => {
            Destroy(bullet);
            Debug.Log("Bullet reached target");
        }));
        // Передача швидкості, шкоди та sequence в компонент Bullet
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
