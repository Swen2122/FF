using UnityEngine;
using DG.Tweening;

public class Projectile2 : MonoBehaviour
{
    // Початкова позиція снаряда
    private Vector3 startPosition;
    // Цільова позиція снаряда
    private Vector3 targetPosition;
    // Дані про навик/здібність
    private SkillData skillData;
    // Прапорець завершення руху
    private bool hasCollided = false;

    // Ініціалізація снаряда з початковою та кінцевою позиціями
    public void Initialize(Vector3 start, Vector3 target, SkillData data)
    {
        startPosition = start;
        targetPosition = target;
        skillData = data;
        Move();
    }

    // Метод руху снаряда
    private void Move()
    {
        // Розрахунок відстані між точками
        float distance = Vector3.Distance(startPosition, targetPosition);
        // Розрахунок тривалості руху
        float duration = distance / skillData.projectileSpeed;

        // Розрахунок параболічної траєкторії
        Vector3[] path = CalculateParabolicPath(duration);

        // Анімація руху з параболічною траєкторією
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(skillData.movementEase));


        // Destroy the projectile after it reaches the target (or a bit after, adjust as needed)
        Destroy(gameObject, duration + 0.1f);  // Prevents projectiles lingering indefinitely
    }

    // Розрахунок параболічної траєкторії руху
    private Vector3[] CalculateParabolicPath(float duration)
    {
        // Розрахунок напрямку руху
        Vector3 direction = (targetPosition - startPosition).normalized;
        // Розрахунок відстані між точками
        float distance = Vector3.Distance(startPosition, targetPosition);

        // Динамічне налаштування висоти параболи залежно від відстані
        float heightMultiplier = Mathf.Clamp(distance / 10f, 0.5f, 2f);
        float maxHeight = skillData.parabolaHeight * heightMultiplier;

        // Розрахунок проміжних точок руху
        Vector3 midPoint1 = Vector3.Lerp(startPosition, targetPosition, 0.33f);
        Vector3 midPoint2 = Vector3.Lerp(startPosition, targetPosition, 0.66f);

        // Збереження горизонтального руху
        midPoint1.x = Mathf.Lerp(startPosition.x, targetPosition.x, 0.33f);
        midPoint2.x = Mathf.Lerp(startPosition.x, targetPosition.x, 0.66f);

        // Підняття проміжних точок
        midPoint1.y += maxHeight * 0.7f;
        midPoint2.y += maxHeight * 0.3f;

        // Повернення масиву точок траєкторії
        return new Vector3[]
        {
            startPosition,
            midPoint1,
            midPoint2,
            targetPosition
        };
    }

    // Метод обробки зіткнення в 2D просторі
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Перевірка, чи зіткнення відбулося з снарядом на тому ж шарі ТА чи снаряд ще не зіткнувся
        if (collision.gameObject.layer == gameObject.layer && !hasCollided)
        {
            // Позначити обидва снаряди як зіткнувшіся
            hasCollided = true;
            Projectile2 otherProjectile = collision.gameObject.GetComponent<Projectile2>();
            if (otherProjectile != null)
            {
                otherProjectile.hasCollided = true;
            }

            // Створення ефекту конвергенції ЛИШЕ ОДИН РАЗ
            if (skillData.impactPrefab != null)
            {
                Instantiate(skillData.impactPrefab, collision.contacts[0].point, Quaternion.identity);
            }

            // Знищення обох снарядів
            Destroy(gameObject);
            Destroy(collision.gameObject);
        }
    }
}