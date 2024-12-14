using UnityEngine;
using System.Collections.Generic;

public class VortexDamageZone : MonoBehaviour
{
    [Header("Pull Settings")]
    [SerializeField] private float pullForce = 5f;
    [SerializeField] private float pullRadius = 5f;

    [Header("Damage Settings")]
    [SerializeField] private float damageRadius = 3f;
    [SerializeField] private float damageInterval = 0.5f;
    [SerializeField] private int damage = 10;
    [SerializeField] private LayerMask targetLayerMask;

    [Header("Lifetime Settings")]
    [SerializeField] private float lifetimeBeforeDestroy = 5f;

    [Header("Visual and Audio")]
    [SerializeField] private ParticleSystem pullEffect;
    [SerializeField] private AudioClip pullingSoundClip;
    [SerializeField] private AudioClip destroySoundClip;

    private AudioSource audioSource;
    private float nextDamageTime;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        if (pullEffect != null) pullEffect.Play();
        if (pullingSoundClip != null)
        {
            audioSource.clip = pullingSoundClip;
            audioSource.loop = true;
            audioSource.Play();
        }

        Destroy(gameObject, lifetimeBeforeDestroy);
    }

    private void OnDestroy()
    {
        if (pullEffect != null) pullEffect.Stop();
        if (destroySoundClip != null && audioSource != null)
        {
            audioSource.loop = false;
            audioSource.PlayOneShot(destroySoundClip);
        }
    }

    private void Update()
    {
        ApplyPullForce();

        if (Time.time >= nextDamageTime)
        {
            ApplyDamageInZone();
            nextDamageTime = Time.time + damageInterval;
        }
    }

    private void ApplyPullForce()
    {
        Collider2D[] objectsToPull = Physics2D.OverlapCircleAll(transform.position, pullRadius, targetLayerMask);

        foreach (var obj in objectsToPull)
        {
            if (obj.TryGetComponent<Rigidbody2D>(out var rb))
            {
                Vector2 pullDirection = (transform.position - obj.transform.position).normalized;
                rb.AddForce(pullDirection * pullForce * Time.fixedDeltaTime, ForceMode2D.Force);
            }
        }
    }

    private void ApplyDamageInZone()
    {
        // Знаходимо ворогів у зоні пошкодження
        Collider2D[] enemiesInZone = Physics2D.OverlapCircleAll(transform.position, damageRadius, targetLayerMask);
        List<GameObject> enemyObjects = new List<GameObject>();

        foreach (var enemy in enemiesInZone)
        {
            enemyObjects.Add(enemy.gameObject);
        }

        // Виклик нанесення шкоди
        if (enemyObjects.Count > 0)
        {
            HitStop.TriggerStop(0.05f, 0.0f);
            Damage.Water(enemyObjects.ToArray(), damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, pullRadius);
    }
}
