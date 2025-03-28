using UnityEngine;

public class CurvedProjectile : BaseProjectile
{
    private float arcHeight;
    private float sideMultiplier;
    private float startTime;
    private Vector2 startPosition;
    private Vector2 perpendicular;
    private bool initialized;
    private float journeyDuration;

    public void Initialize(ProjectileData data, Vector2 target, Element element, float height, float side)
    {
        base.Initialize(data, target, element);
        arcHeight = height;
        sideMultiplier = side;
        startPosition = transform.position;
        startTime = Time.time;

        // Обчислюємо перпендикулярний вектор до напрямку руху
        Vector2 direction = (target - startPosition).normalized;
        perpendicular = new Vector2(-direction.y, direction.x) * sideMultiplier;
        
        // Розраховуємо тривалість подорожі на основі відстані та швидкості
        float distance = Vector2.Distance(startPosition, target);
        journeyDuration = distance / speed;

        initialized = true;
    }
    private void Update()
    {
        Move();
    }

    protected override void Move()
    {
        if (!initialized) return;

        float progress = (Time.time - startTime) / journeyDuration;
        
        if (progress >= 1f)
        {
            OnProjectileReachedTarget();
            return;
        }

        // Базова позиція по прямій
        Vector2 basePosition = Vector2.Lerp(startPosition, targetPosition, progress);
        
        // Крива висоти: підйом до max і плавний спад
        float peakOffset = Mathf.Sin(progress * Mathf.PI) * arcHeight;
        
        // Додаємо зміщення перпендикулярно до напрямку руху
        Vector2 offset = perpendicular * peakOffset;
        Vector2 finalPosition = basePosition + offset;
        
        transform.position = finalPosition;

        // Оновлюємо поворот снаряду
        UpdateRotation(progress);
    }
    protected override void OnHit(Collider2D other)
    {
        if (other.TryGetComponent<ICanHit>(out var damageable))
        {
            damageable.TakeHit(damage, currentElement);
        }
    }
    private void UpdateRotation(float progress)
    {
        // Обчислюємо наступну позицію для визначення напрямку
        float nextProgress = Mathf.Min(progress + 0.01f, 1f);
        Vector2 nextBasePosition = Vector2.Lerp(startPosition, targetPosition, nextProgress);
        float nextPeakOffset = Mathf.Sin(nextProgress * Mathf.PI) * arcHeight;
        Vector2 nextOffset = perpendicular * nextPeakOffset;
        Vector2 nextPosition = nextBasePosition + nextOffset;

        // Обчислюємо напрямок руху
        Vector2 direction = (nextPosition - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }

    protected override void OnProjectileReachedTarget()
    {
        Destroy(gameObject, 0.1f);
    }
}