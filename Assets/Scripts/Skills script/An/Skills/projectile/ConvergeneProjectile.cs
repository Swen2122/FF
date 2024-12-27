using UnityEngine;
using DG.Tweening;

/// <summary>
/// Enhanced projectile implementation for convergence-type skills with elemental support
/// </summary>
public class ConvergenceProjectile : MonoBehaviour
{
    private ProjectileData projectileData;
    private ConvergenceSkillData convergenceData;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private LayerMask targetLayer;
    private bool hasCollided;
    private Element currentElement;
    private static bool isVortexCreated; // Статичне поле для відстеження створення вортексу

    // Налаштування руху
    private int mirrorDirection;
    private bool isHorizontalDominant;
    private Transform sourceTransform;

    public void Initialize(ProjectileData data, ConvergenceSkillData convData, LayerMask layer,
        Transform source, Vector2 target, int convergenceDirection, bool horizontalDominant, Element element)
    {
        projectileData = data;
        convergenceData = convData;
        targetLayer = layer;
        sourceTransform = source;
        startPosition = transform.position;
        targetPosition = target;
        mirrorDirection = convergenceDirection;
        isHorizontalDominant = horizontalDominant;
        currentElement = element;
        hasCollided = false;

        InitializeProjectile();
    }

    private void InitializeProjectile()
    {
        PlayInitialEffects();
        StartProjectileMovement();
    }

    private void PlayInitialEffects()
    {
        if (projectileData.spawnEffect != null)
        {
            Instantiate(projectileData.spawnEffect, transform.position, Quaternion.identity);
        }

        if (projectileData.launchSound != null)
        {
            AudioSource.PlayClipAtPoint(projectileData.launchSound, transform.position);
        }

        if (projectileData.trailEffect != null)
        {
            Instantiate(projectileData.trailEffect, transform);
        }
    }

    private void StartProjectileMovement()
    {
        float distance = Vector3.Distance(startPosition, targetPosition);
        float duration = distance / projectileData.speed;

        Vector3[] path = CalculateParabolicPath(duration);

        transform.DOPath(path, duration, PathType.CatmullRom)
            .SetEase(projectileData.moveEase)
            .OnComplete(() =>
            {
                if (!hasCollided)
                {
                    HandleProjectileTimeout();
                }
            });
    }

    private Vector3[] CalculateParabolicPath(float duration)
    {
        Vector3 direction = (targetPosition - startPosition).normalized;
        float distance = Vector3.Distance(startPosition, targetPosition);
        float heightMultiplier = Mathf.Clamp(distance / 10f, 0.5f, 2f);
        float maxHeight = projectileData.parabolaHeight * heightMultiplier;

        Vector3 p1 = startPosition;
        Vector3 p4 = targetPosition;

        // Calculate intermediate control points
        float t1 = 0.33f;
        float t2 = 0.66f;
        Vector3 p2 = Vector3.Lerp(startPosition, targetPosition, t1);
        Vector3 p3 = Vector3.Lerp(startPosition, targetPosition, t2);

        // Apply mirrored offset based on movement direction
        if (isHorizontalDominant)
        {
            p2.y += maxHeight * mirrorDirection;
            p3.y += maxHeight * mirrorDirection * 0.5f;
        }
        else
        {
            p2.x += maxHeight * mirrorDirection;
            p3.x += maxHeight * mirrorDirection * 0.5f;
        }

        return new Vector3[] { p1, p2, p3, p4 };
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasCollided) return;

        if (other.TryGetComponent<ConvergenceProjectile>(out var otherProjectile))
        {
            HandleConvergenceCollision(otherProjectile);
        }
        else if (((1 << other.gameObject.layer) & targetLayer) != 0)
        {
            HandleTargetCollision(other);
        }
    }
    private void HandleTargetCollision(Collider2D target)
    {
        if (target.TryGetComponent<ICanHit>(out var damageable))
        {
            damageable.TakeHit(projectileData.damage);
        }

        CreateImpactEffect(transform.position);
        Destroy(gameObject);
    }

    private void CreateConvergenceEffect(Vector3 position)
    {
        if (projectileData.hitEffect != null)
        {
            Instantiate(projectileData.hitEffect, position, Quaternion.identity);
        }

        if (projectileData.hitSound != null)
        {
            AudioSource.PlayClipAtPoint(projectileData.hitSound, position);
        }
    }

    private void CreateImpactEffect(Vector3 position)
    {
        if (projectileData.hitEffect != null)
        {
            Instantiate(projectileData.hitEffect, position, Quaternion.identity);
        }

        if (projectileData.hitSound != null)
        {
            AudioSource.PlayClipAtPoint(projectileData.hitSound, position);
        }
    }

    private void HandleProjectileTimeout()
    {
        CreateImpactEffect(transform.position);
        Destroy(gameObject);
    }
    private void HandleConvergenceCollision(ConvergenceProjectile otherProjectile)
    {
        if (otherProjectile.hasCollided || isVortexCreated) return;

        // Перевіряємо відстань між снарядами
        float distance = Vector3.Distance(transform.position, otherProjectile.transform.position);
        if (distance <= convergenceData.pullRadius)
        {
            hasCollided = true;
            otherProjectile.hasCollided = true;
            isVortexCreated = true;

            CreateVortex(transform.position);

            if (convergenceData.convergenceEffect != null)
            {
                Instantiate(convergenceData.convergenceEffect, transform.position, Quaternion.identity);
            }

            if (convergenceData.convergenceSound != null)
            {
                AudioSource.PlayClipAtPoint(convergenceData.convergenceSound, transform.position);
            }

            if (otherProjectile != null)
            {
                Destroy(otherProjectile.gameObject);
            }
            Destroy(gameObject);
        }
    }

    private void CreateVortex(Vector3 position)
    {
        if (convergenceData.vortexPrefab != null)
        {
            GameObject vortexObj = Instantiate(convergenceData.vortexPrefab, position, Quaternion.identity);
            if (vortexObj.TryGetComponent<VortexDamageZone>(out var vortex))
            {
                // Налаштовуємо вортекс з даних конвергенції
                vortex.Initialize(convergenceData, targetLayer, currentElement);
            }
        }
    }
}