using UnityEngine;
using DG.Tweening;
public class EnhancedProjectile : MonoBehaviour
{
    private ProjectileData data;
    private LayerMask targetLayer;
    private Transform source;
    private Vector2 targetPosition;
    private bool isInitialized = false;
    private bool isConvergenceProjectile = false;
    private int convergenceDirection = 1;
    private bool isHorizontalDominant = false;

    public void Initialize(ProjectileData projectileData, LayerMask layer, Transform sourceTransform,
        Vector2 target, bool isConvergence = false, int convergeDir = 1, bool horizontalDominant = false)
    {
        data = projectileData;
        targetLayer = layer;
        source = sourceTransform;
        targetPosition = target;
        isConvergenceProjectile = isConvergence;
        convergenceDirection = convergeDir;
        isHorizontalDominant = horizontalDominant;
        isInitialized = true;

        HandleInitialEffects();
        StartProjectileMovement();
    }

    private void HandleInitialEffects()
    {
        if (data.spawnEffect != null)
        {
            Instantiate(data.spawnEffect, transform.position, Quaternion.identity);
        }

        if (data.launchSound != null)
        {
            AudioSource audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = data.launchSound;
            audioSource.Play();
        }

        if (data.trailEffect != null)
        {
            Instantiate(data.trailEffect, transform);
        }
    }

    private void StartProjectileMovement()
    {
        if (data.useParabolicPath && isConvergenceProjectile)
        {
            StartParabolicMovement();
        }
        else
        {
            StartLinearMovement();
        }
    }

    private void StartLinearMovement()
    {
        float distance = Vector2.Distance(transform.position, targetPosition);
        float duration = distance / data.speed;

        transform.DOMove(targetPosition, duration)
            .SetEase(data.moveEase)
            .OnComplete(() => OnProjectileReachedTarget());
    }

    private void StartParabolicMovement()
    {
        float distance = Vector2.Distance(transform.position, targetPosition);
        float duration = distance / data.speed;
        Vector3[] path = CalculateParabolicPath(duration);

        transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(data.moveEase)
            .OnComplete(() => OnProjectileReachedTarget());
    }

    private Vector3[] CalculateParabolicPath(float duration)
    {
        Vector3 startPos = transform.position;
        float distance = Vector2.Distance(startPos, targetPosition);
        float heightMultiplier = Mathf.Clamp(distance / 10f, 0.5f, 2f);
        float maxHeight = data.parabolaHeight * heightMultiplier;

        // Розрахунок контрольних точок параболи
        Vector3 p1 = startPos;
        Vector3 p4 = targetPosition;
        float t1 = 0.33f;
        float t2 = 0.66f;

        Vector3 p2 = Vector3.Lerp(startPos, targetPosition, t1);
        Vector3 p3 = Vector3.Lerp(startPos, targetPosition, t2);

        // Застосування зміщення для ефекту конвергенції
        if (isHorizontalDominant)
        {
            p2.y += maxHeight * convergenceDirection;
            p3.y += maxHeight * convergenceDirection * 0.5f;
        }
        else
        {
            p2.x += maxHeight * convergenceDirection;
            p3.x += maxHeight * convergenceDirection * 0.5f;
        }

        return new Vector3[] { p1, p2, p3, p4 };
    }

    private void OnProjectileReachedTarget()
    {
        HandleImpactEffects();
        Destroy(gameObject);
    }

    private void HandleImpactEffects()
    {
        if (data.hitEffect != null)
        {
            Instantiate(data.hitEffect, transform.position, Quaternion.identity);
        }

        if (data.hitSound != null)
        {
            AudioSource.PlayClipAtPoint(data.hitSound, transform.position);
        }

        if (data.useScreenShake)
        {
            // Implement screen shake using your preferred method
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isInitialized) return;

        if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            HandleCollision(other);
        }
    }

    private void HandleCollision(Collider2D other)
    {
        if (other.TryGetComponent<ICanHit>(out var health))
        {
            health.TakeHit(data.damage);
        }

        // Special handling for convergence projectiles
        if (isConvergenceProjectile && other.TryGetComponent<EnhancedProjectile>(out var otherProjectile))
        {
            HandleConvergenceCollision(otherProjectile);
        }
        else
        {
            OnProjectileReachedTarget();
        }
    }

    private void HandleConvergenceCollision(EnhancedProjectile otherProjectile)
    {
        if (otherProjectile.isConvergenceProjectile)
        {
            OnProjectileReachedTarget();
            Destroy(otherProjectile.gameObject);
        }
    }
}